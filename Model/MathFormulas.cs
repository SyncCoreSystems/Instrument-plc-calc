using Instrument_Plc_Converter.Model.Interfaces;


namespace Instrument_Plc_Converter.Model;

public class MathFormulas : IScalingMath
{
    
    #region Plc Raw Value and Engineering Value

    // PLC Raw → Engineering Value
    public double RawToEngineering(
        double rawValue,
        double rawMin,
        double rawMax,
        double engineeringMin,
        double engineeringMax
    )
    {
        var engineeringValue = ((rawValue - rawMin) * (engineeringMax - engineeringMin)) /
            (rawMax - rawMin) + engineeringMin;

        return engineeringValue;
    }

    // Engineering → PLC Raw
    public double EngineeringToRaw(double engineeringValue,
        double rawMin,
        double rawMax,
        double engineeringMin,
        double engineeringMax
    )
    {
        var rawValue = ((engineeringValue - engineeringMin) * (rawMax - rawMin)) /
            (engineeringMax - engineeringMin) + rawMin;

        return rawValue;
    }

    #endregion

    #region Calculation with electrical signal

    // Current or Voltage → Engineering Value
    public double ElectricalToEngineering(
        double electricalValue,
        int lrv,
        double span,
        double engineeringMin,
        double engineeringMax
    )
    {
        var engineeringValue = ((electricalValue - lrv) * (engineeringMax - engineeringMin)) /
            span + engineeringMin;

        return engineeringValue;
    }

    // Engineering Value → Current or Voltage
    public double EngineeringToElectrical(
        double engineeringValue,
        int lrv,
        double span,
        double engineeringMin,
        double engineeringMax
    )
    {
        var electricalValue = ((engineeringValue - engineeringMin) * span) /
            (engineeringMax - engineeringMin) + lrv;

        return electricalValue;
    }

    #endregion
}