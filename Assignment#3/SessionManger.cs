namespace Assignment_3;

public static class SessionManger
{
    static List<Session> Sessions = new List<Session>();
    static string? currentToken;

    static public Session? GetSession(string token)
    {
        foreach (var session in Sessions)
        {
            if (session.Token == token)
            {
                return session;
            }
        }
        return null;
    }
    static public Session? GetCurrentSession()
    {
        return GetSession(currentToken ?? "") ?? null;
    }
    static public void AddSession(User user, string token, bool isRemembered)
    {
        Sessions.Add(new Session(user, token, isRemembered));
    }
    static public void RemoveSession(string token)
    {
        Sessions.RemoveAll(session => session.Token == token);
    }
    static public bool SwitchSession(string token)
    {
        Session currentSession = GetSession(token) ?? throw new Exception("Session not found");
        foreach (var session in Sessions)
        {
            if (session.Token == token)
            {
                currentSession.SwitchSession();
                if (session.Login())
                {
                    currentToken = session.Token;
                    session.UpdateLastActionTime();
                    return true;
                }
                return false;
            }
        }
        return false;
    }

    static public bool checkAuthorized(User user)
    {
        return GetCurrentSession()?.User == user;
    }
}
