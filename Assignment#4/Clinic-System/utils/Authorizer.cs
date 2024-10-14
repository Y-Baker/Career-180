namespace ClinicSystem;

internal class Authorizer
{
    public static bool checkAuthorized(Session session)
    {
        session.CheckSession();
        if (session.IsLoggedIn)
        {
            session.LastActionTime = DateTime.Now;
            return true;
        }
        else
            return false;
    }

    public static bool checkAuthorized(Account account)
    {
        Session? session =  Session.GetCurrentSession();
        if (session is null || !session.IsLoggedIn)
        {
            Console.WriteLine("You need to login first");
            return false;
        }
        if (!checkAuthorized(session))
            return false;
        if (session.Account != account)
            return false;
        return true;
    }
}
