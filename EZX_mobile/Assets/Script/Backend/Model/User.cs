public class User
{
    public string username;
    public string access_token;
    public int weight;
    public int height;
    public int age;

    public double BMI()
    {
        float height = (float)(this.height / 100.0);
        double bmi = weight / (height * height);

        // Adjust BMI for age
        if (age < 20)
        {
            bmi *= 1.1;
        }
        else if (age > 65)
        {
            bmi *= 0.9;
        }

        return bmi;
    }
}
