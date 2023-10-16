
public interface IValidator<T>
{
    public void Validate(T target);
}



public class User
{
    public int UserId { get; set; }
}

public static class Assert
{
    public static bool GreaterThen(int value, int min)
    {
        return value > min;
    }
}


public class UserValidator : IValidator<User>
{
    public void Validate(User target)
    {


        
    }
}




class Program
{
    public static void Main(string[] args)
    {

    }
}