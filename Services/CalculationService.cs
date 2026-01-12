using Instrument_Plc_Converter.Model;
using Instrument_Plc_Converter.ViewModel;

namespace Instrument_Plc_Converter.Services;

public static class CalculationService
{
    private static readonly MathFormulas _mathFormulas = new();
    private static readonly CalculationApp _calculationApp = new();

    #region Assign values for each property

    private static double InputValue => _calculationApp.CurrentValue;
    public static int RawMin => _calculationApp.SelectedProfile.RawMin; // check this, null value
    public static int RawMax => _calculationApp.SelectedProfile.RawMax; // check this, null value
    private static double EngineeringMin => _calculationApp.LrvEngineering;
    private static double EngineeringMax => _calculationApp.UrvEngineering;
    private static int ElectricalMin => _calculationApp.LrvSignal;
    private static int ElectricalMax => _calculationApp.UrvSignal;
    private static double ElectricalSpan => _calculationApp.ElectricSpanResult;
    private static double EngineeringSpanResult => _calculationApp.EngineeringSpanResult;

    #endregion

    // PLC Raw → Engineering Value
    public static double GetEngineeringValueFromPlc()
    {
        return _mathFormulas.RawToEngineering(InputValue, RawMin, RawMax, EngineeringMin, EngineeringMax);
    }

    // Engineering → PLC Raw
    public static double GetRawValueFromEngineering()
    {
        return _mathFormulas.EngineeringToRaw(InputValue, RawMin, RawMax, EngineeringMin,
            EngineeringMax);
    }

    // Current or Voltage → Engineering Value
    public static double GetEngineeringValueFromElectrical()
    {
        return _mathFormulas.ElectricalToEngineering(InputValue,
            ElectricalMin, ElectricalSpan, EngineeringMin,
            EngineeringMax);
    }

    // Engineering Value → Current or Voltage
    public static double GetElectricalValueFromEngineering()
    {
        return _mathFormulas.EngineeringToElectrical(
            InputValue,
            ElectricalMin,
            EngineeringSpanResult,
            EngineeringMin,
            EngineeringMax
        );
    }
}