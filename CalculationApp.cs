using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using Instrument_Plc_Converter.Model;
using Instrument_Plc_Converter.Services;
using Instrument_Plc_Converter.ViewModel;

namespace Instrument_Plc_Converter;

public class CalculationApp : ViewModelBase
{
    public double Test { get;  set; }
    public double Test2 { get; set; }
    #region Relay Commands

    public IRelayCommand ExecuteMath { get; }

    #endregion

    #region Instance Classes

    private readonly SignalInstrument _signalInstrument = new();
    private readonly EngineeringInstrument _engineeringInstrument = new();
    private readonly InputValue _inputValue = new();

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

    #endregion

    #region Instrument Signal

    public string SignalTypeReference
    {
        get => _signalInstrument.SignalType;
        set => _signalInstrument.SignalType = value;
    }

    public int LrvSignal
    {
        get => _signalInstrument.LowerRangeValue;
        set
        {
            _signalInstrument.LowerRangeValue = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ElectricSpanResult));
        }
    }

    public int UrvSignal
    {
        get => _signalInstrument.UpperRangeValue;
        set
        {
            _signalInstrument.UpperRangeValue = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ElectricSpanResult));
        }
    }

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

    void ExecuteCalculation()
    {
        switch (_inputValue.InputType)
        {
            case "Engineering":
                Test = CalculationService.GetElectricalValueFromEngineering();
                break;
            case "Signal":
                CalculationService.GetEngineeringValueFromElectrical();
                break;
            case "Raw":
                break;
        }
        Debug.WriteLine(Test);
        Debug.WriteLine(Test2);
        Debug.WriteLine($"SelectedProfile: {SelectedProfile}");
        Debug.WriteLine($"ElectricalSpan={ElectricSpanResult}, EngineeringSpan={EngineeringSpanResult}");
        Debug.WriteLine($"InputValue: {CurrentValue}");
        Debug.WriteLine($"InputValue: {SelectedType}");
        Debug.WriteLine($"InputValue: {CalculationService.RawMin}");
        Debug.WriteLine($"InputValue: {CalculationService.RawMax}");


    }
}