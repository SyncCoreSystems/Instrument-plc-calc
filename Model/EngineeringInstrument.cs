namespace Instrument_Plc_Converter.Model;

public class EngineeringInstrument
{
    public string Unit { get; set; } = string.Empty;
    public double LowerRangeValue { get; set; }
    public double UpperRangeValue { get; set; }
    public double Span  =>  UpperRangeValue - LowerRangeValue; 
    
}