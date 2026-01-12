using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Instrument_Plc_Converter.Model;
using Instrument_Plc_Converter.ViewModel;

namespace Instrument_Plc_Converter;

public class CalculationApp : ViewModelBase
{
    #region Relay Commands

    public IRelayCommand ExecuteMath { get; }

    #endregion

    #region Instance Classes

    // private readonly SignalInstrument _signalInstrument = new();
    private readonly EngineeringInstrument _engineeringInstrument = new();
    private readonly InputValue _inputValue = new();
    private readonly MathFormulas _mathFormulas = new();

    #endregion

    public CalculationApp()
    {
        ExecuteMath = new RelayCommand(ExecuteCalculation);
    }

    #region Collections

    public ObservableCollection<PlcProfile> ProfileList { get; } = new()
    {
        new PlcProfile { ProfileName = "Siemens", RawMin = 0, RawMax = 27648 },
        new PlcProfile { ProfileName = "Delta", RawMin = 0, RawMax = 4000 },
        new PlcProfile { ProfileName = "AllenBradley", RawMin = 0, RawMax = 65847 }
    };

    public ObservableCollection<InputValue> InputType { get; } = new()
    {
        new InputValue { InputType = "Engineering", CurrentValue = 0 },
        new InputValue { InputType = "Signal", CurrentValue = 0 },
        new InputValue { InputType = "Raw", CurrentValue = 0 },
    };

    public ObservableCollection<SignalInstrument> InstrumentSignal { get; } = new()
    {
        new SignalInstrument { SignalType = "4-20mA", LowerRangeValue = 4, UpperRangeValue = 20 },
        new SignalInstrument { SignalType = "0-20mA", LowerRangeValue = 0, UpperRangeValue = 20 },
        new SignalInstrument { SignalType = "0-10V", LowerRangeValue = 0, UpperRangeValue = 10 },
        new SignalInstrument { SignalType = "1-5V", LowerRangeValue = 1, UpperRangeValue = 5 },
        new SignalInstrument { SignalType = "2-10V", LowerRangeValue = 2, UpperRangeValue = 10 }
    };

    #endregion

    #region Instrument Signal

    private SignalInstrument _signalInstrument;

    public SignalInstrument SignalTypeReference
    {
        get => _signalInstrument;
        set
        {
            _signalInstrument = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ElectricSpanResult));
        }
    }

    public int LrvSignal => _signalInstrument.LowerRangeValue;

    public int UrvSignal => _signalInstrument.UpperRangeValue;

    public double ElectricSpanResult => _signalInstrument.Span;

    #endregion

    #region Instrument Engineering

    public string UnitEngineering
    {
        get => _engineeringInstrument.Unit;
        set => _engineeringInstrument.Unit = value;
    }

    public double LrvEngineering
    {
        get => _engineeringInstrument.LowerRangeValue;
        set
        {
            _engineeringInstrument.LowerRangeValue = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(EngineeringSpanResult));
        }
    }

    public double UrvEngineering
    {
        get => _engineeringInstrument.UpperRangeValue;
        set
        {
            _engineeringInstrument.UpperRangeValue = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(EngineeringSpanResult));
        }
    }

    public double EngineeringSpanResult => _engineeringInstrument.Span;

    #endregion

    #region Input Value

    public string SelectedType
    {
        get => _inputValue.InputType;
        set => _inputValue.InputType = value;
    }

    public double CurrentValue
    {
        get => _inputValue.CurrentValue;
        set => _inputValue.CurrentValue = value;
    }

    #endregion

    #region Plc Profiles

    private PlcProfile? _selectedProfile;

    public PlcProfile SelectedProfile
    {
        get => _selectedProfile!;
        set
        {
            _selectedProfile = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Execute Calculation Formulas

    // Electrical Signal Value
    private double _electricSignalValue;

    public double ElectricSignalValue
    {
        get => _electricSignalValue;
        set
        {
            _electricSignalValue = value;
            OnPropertyChanged();
        }
    }

    // Engineering Signal
    private double _engineeringValue;

    public double EngineeringValue
    {
        get => _engineeringValue;
        set
        {
            _engineeringValue = value;
            OnPropertyChanged();
        }
    }

    private double _rawValue;

    public double RawValue
    {
        get => _rawValue;
        set
        {
            _rawValue = value;
            OnPropertyChanged();
        }
    }

    // Methods for execute if the input type is Engineering
    void EngineeringMode()
    {
        try
        {
            if (CurrentValue > UrvEngineering || CurrentValue < LrvEngineering)
            {
                MessageBox.Show("The input value is out of instrument engineering range.");
            }

            if (CurrentValue <= UrvEngineering && CurrentValue >= LrvEngineering)
            {
                // Electrical Value
                ElectricSignalValue = _mathFormulas.EngineeringToElectrical(
                    CurrentValue,
                    LrvSignal,
                    ElectricSpanResult,
                    LrvEngineering,
                    UrvEngineering);
                // Raw Plc Value
                RawValue = _mathFormulas.EngineeringToRaw(
                    CurrentValue,
                    SelectedProfile.RawMin,
                    SelectedProfile.RawMax,
                    LrvEngineering,
                    UrvEngineering
                );
                // Engineering Value is equal to Input Value
                EngineeringValue = CurrentValue;
            }
        }
        catch (Exception e)
        {
            MessageBox.Show($"{e}");
        }
    }

    // Methods for execute if the input type is Signal
    void SignalMode()
    {
        if (CurrentValue > UrvSignal || CurrentValue < LrvSignal)
        {
            MessageBox.Show("The input value is out of instrument electrical range.");
        }

        if (CurrentValue <= UrvSignal && CurrentValue >= LrvSignal)
        {
            // Engineering Value
            EngineeringValue = _mathFormulas.ElectricalToEngineering(
                CurrentValue,
                LrvSignal,
                ElectricSpanResult,
                LrvEngineering,
                UrvEngineering
            );
            // Raw Value
            RawValue = _mathFormulas.ElectricalToRaw(
                CurrentValue,
                LrvSignal,
                UrvSignal,
                SelectedProfile.RawMin,
                SelectedProfile.RawMax
            );
            // Signal is equal to Input Value
            ElectricSignalValue = CurrentValue;
        }
    }

    // Methods for execute if the input type is Raw
    void RawMode()
    {
        if (CurrentValue > SelectedProfile.RawMax || CurrentValue < SelectedProfile.RawMin)
        {
            MessageBox.Show("The input value is out of plc range.");
        }

        if (CurrentValue <= SelectedProfile.RawMax && CurrentValue >= SelectedProfile.RawMin)
        {
            // Engineering Value
            EngineeringValue = _mathFormulas.RawToEngineering(
                CurrentValue,
                SelectedProfile.RawMin,
                SelectedProfile.RawMax,
                LrvEngineering,
                UrvEngineering
            );

            // Signal Value
            ElectricSignalValue = _mathFormulas.RawToElectrical(
                CurrentValue,
                SelectedProfile.RawMin,
                SelectedProfile.RawMax,
                LrvSignal,
                UrvSignal
            );
            // Raw Plc Value is equal to Input Value
            RawValue = CurrentValue;
        }
    }

    // Method for execute math formulas
    void ExecuteCalculation()
    {
        #region Validations

        if (SignalTypeReference == null)
        {
            MessageBox.Show("Specify the electrical instrument signal type");
        }

        if (SelectedProfile == null)
        {
            MessageBox.Show("Specify the brand plc scaling");
        }

        if (string.IsNullOrEmpty(SelectedType))
        {
            MessageBox.Show("Specify the input type");
        }

        if (string.IsNullOrEmpty(UnitEngineering))
        {
            MessageBox.Show("Specify the Engineering Unit");
        }

        if (LrvEngineering == 0 || UrvEngineering == 0)
        {
            MessageBox.Show("Engineering values cannot be empty or equal to 0.");
        }

        if (UrvEngineering <= LrvEngineering)
            MessageBox.Show("Engineering URV value must be greater than LRV.");

        #endregion

        switch (SelectedType)
        {
            case "Engineering":
                if (LrvEngineering != 0 && UrvEngineering != 0 && UrvEngineering > LrvEngineering)
                {
                    EngineeringMode();
                }

                break;
            case "Signal":
                SignalMode();
                break;
            case "Raw":
                RawMode();
                break;
        }
    }

    #endregion
}