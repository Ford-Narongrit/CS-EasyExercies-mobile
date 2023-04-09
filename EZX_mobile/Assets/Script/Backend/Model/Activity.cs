public class Activity
{
    public string date;
    public int burned_cal; //Kcal
    public int time; //Seccond

    public Activity(string date, int burned_cal, int time)
    {
        this.date = date;
        this.time = time;
        this.burned_cal = burned_cal;
    }
}