namespace ClinicSystem;

public class Appoiment
{
    public DateTime Time { get; set; }
    public Patient Patient { get; set; }
    public Assistant Assistant { get; set; }
    public Doctor Doctor { get; set; }
    public double Price { get; set; }
    public AppoimentState State { get; set; }
    public bool AppoimentIsPaid { get; set; }

    public Appoiment(DateTime time, Patient patient, Assistant assistant, Doctor doctor, double price, bool appoimentIsPaid=false)
    {
        Time = time;
        Patient = patient;
        Assistant = assistant;
        Doctor = doctor;
        Price = price;
        State = AppoimentState.Pending;
        AppoimentIsPaid = appoimentIsPaid;
    }
    public static bool TimeIsAvailable(DateTime time, Department department)
    {
        List<Appoiment> appoiments = MemoryStorage.Instance.GetAppoimentsByDepartment(department);

        foreach (Appoiment appoiment in appoiments)
        {
            if (appoiment.Time.AddMinutes(-30) <= time && time <= appoiment.Time.AddMinutes(30))
            {
                return false; 
            }
        }
        return true;
    }

    public static string HeadView()
    {
        return $"{"Time",-30} {"Patient",-20} {"State",-20} {"Paid",-20} {"Price",-20}";
    }
    public string View()
    {
        return $"{Time,-30} {Patient.Name,-20} {State,-20} {AppoimentIsPaid,-20} {Price,-20}";
    }
}
