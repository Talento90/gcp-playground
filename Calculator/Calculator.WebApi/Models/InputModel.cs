namespace Calculator.WebApi.Models
{
    public enum Operation {Addition, Subtraction}
    public record InputModel(int Num1, int Num2, Operation Operation);
}