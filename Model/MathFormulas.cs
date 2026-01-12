using System.Diagnostics;
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
        return Math.Round(((rawValue - rawMin) * (engineeringMax - engineeringMin)) /
            (rawMax - rawMin) + engineeringMin, 1);
    }

    // PLC Raw → Current or Voltage
    public double RawToElectrical(
        double rawValue,
        double rawMin,
        double rawMax,
        double electricalMin,
        double electricalMax
    )
    {
        return Math.Round(((rawValue - rawMin) * (electricalMax - electricalMin)) / (rawMax - rawMin) + electricalMin,
            1);
    }

    // Engineering → PLC Raw
    public double EngineeringToRaw(double engineeringValue,
        double rawMin,
        double rawMax,
        double engineeringMin,
        double engineeringMax
    )
    {
        return Math.Round(((engineeringValue - engineeringMin) * (rawMax - rawMin)) /
            (engineeringMax - engineeringMin) + rawMin);
    }

    #endregion

    #region Calculation with electrical signal

    // Current or Voltage → Engineering Value
    public double ElectricalToEngineering(
        double electricalValue,
        double electricalLrv,
        double electricalSpan,
        double engineeringMin,
        double engineeringMax
    )
    {
        return Math.Round(((electricalValue - electricalLrv) * (engineeringMax - engineeringMin)) /
            electricalSpan + engineeringMin, 1);
    }

    // Current or Voltage → Plc Raw
    public double ElectricalToRaw(
        double electricalValue,
        double electricalLrv,
        double electricalUrv,
        double rawMin,
        double rawMax
    )
    {
        return Math.Round(((electricalValue - electricalLrv) * (rawMax - rawMin)) / (electricalUrv - electricalLrv) +
                          rawMin);
    }

    // Engineering Value → Current or Voltage
    public double EngineeringToElectrical(
        double engineeringValue,
        double electricalLrv,
        double electricalSpan,
        double engineeringMin,
        double engineeringMax
    )
    {
        return Math.Round(((engineeringValue - engineeringMin) * electricalSpan) /
            (engineeringMax - engineeringMin) + electricalLrv, 1);
    }

    #endregion
}