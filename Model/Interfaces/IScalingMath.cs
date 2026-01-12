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

    double EngineeringToRaw(
        double engineeringValue, // Current Process Value, physical variable
        double rawMin, // Raw Plc analog signal minimum
        double rawMax, // Raw Plc analog signal maximum
        double engineeringMin, // Process value LRV, physical variable
        double engineeringMax // Process value URV, physical variable
    );

    double ElectricalToEngineering(
        double electricalValue, // Current or Voltage process value
        int lrv, // Signal Lower Range Value
        double span, // Signal Span
        double engineeringMin, // Process value LRV, physical variable
        double engineeringMax // Process value URV, physical variable
    );

    double EngineeringToElectrical(
        double engineeringValue, // Current Process Value, physical variable
        int lrv, // Signal Lower Range Value
        double electricalSpan, // Signal Span
        double engineeringMin, // Process value LRV, physical variable
        double engineeringMax // Process value URV, physical variable
        );
}