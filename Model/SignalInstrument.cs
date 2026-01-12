namespace Instrument_Plc_Converter.Model;

public class SignalInstrument
{
    public string SignalType {get; set;} = string.Empty;
    public int LowerRangeValue {get; set;}
    public int UpperRangeValue {get; set;}
    public double Span => UpperRangeValue - LowerRangeValue;
}