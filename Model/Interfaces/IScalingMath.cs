namespace Instrument_Plc_Converter.Model.Interfaces;

public interface IScalingMath
{
    double RawToEngineering(
        double rawValue, // Value from plc analog signal
        double rawMin, // Raw Plc analog signal minimum
        double rawMax, // Raw Plc analog signal maximum
        double engineeringMin, // Process value LRV, physical variable
        double engineeringMax // Process value URV, physical variable
    );

    double RawToElectrical(
        double rawValue,
        double rawMin,
        double rawMax,
        double electricalMin,
        double electricalMax
    );

    double EngineeringToRaw(
        double engineeringValue, // Current Process Value, physical variable
        double rawMin, // Raw Plc analog signal minimum
        double rawMax, // Raw Plc analog signal maximum
        double engineeringMin, // Process value LRV, physical variable
        double engineeringMax // Process value URV, physical variable
    );

    double ElectricalToEngineering(
        double electricalValue, // Current or Voltage process value
        double electricalLrv, // Signal Lower Range Value
        double electricalSpan, // Signal Span
        double engineeringMin, // Process value LRV, physical variable
        double engineeringMax // Process value URV, physical variable
    );

    double ElectricalToRaw(
        double electricalValue,
        double electricalLrv,
        double electricalUrv,
        double rawMin,
        double rawMax
    );

    double EngineeringToElectrical(
        double engineeringValue, // Current Process Value, physical variable
        double electricalLrv, // Signal Lower Range Value
        double electricalSpan, // Signal Span
        double engineeringMin, // Process value LRV, physical variable
        double engineeringMax // Process value URV, physical variable
    );
}