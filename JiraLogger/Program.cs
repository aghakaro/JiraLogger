using JiraLogger;

bool isEvenNum = DateTime.Now.Day % 2 == 0;
DateTime firstDayCurrentMonth;
string started;


var httpClient = new HttpClient();
var jira = new JiraLogger.JiraClient(httpClient);

for (int i = 0; i < DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month); i++)
{
    if (i == 0)
    {
        firstDayCurrentMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        if (firstDayCurrentMonth.DayOfWeek != DayOfWeek.Sunday && firstDayCurrentMonth.DayOfWeek != DayOfWeek.Saturday)
        {
            started = firstDayCurrentMonth.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK");
            started = started[..(started.IndexOf('T') + 1)] + "06:00:00.000+0400";
            _ = await jira.PostAsync("rest/api/3/issue/STAT-4559/worklog", new Body { started = started, timeSpent = "1h" });
            _ = await jira.PostAsync("rest/api/3/issue/STAT-4578/worklog", new Body { started = started, timeSpent = isEvenNum ? "2h" : "3h" });
            _ = await jira.PostAsync("rest/api/3/issue/STAT-4583/worklog", new Body { started = started, timeSpent = isEvenNum ? "3h" : "2h" });

        }
    }
    else
    {
        var start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, i).AddDays(1);
        if (start.DayOfWeek != DayOfWeek.Sunday && start.DayOfWeek != DayOfWeek.Saturday)
        {
            started = start.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK");
            started = started[..(started.IndexOf('T') + 1)] + "06:00:00.000+0400";
            _ = await jira.PostAsync("rest/api/3/issue/STAT-4559/worklog", new Body { started = started, timeSpent = "1h" });
            _ = await jira.PostAsync("rest/api/3/issue/STAT-4578/worklog", new Body { started = started, timeSpent = isEvenNum ? "2h" : "3h" });
            _ = await jira.PostAsync("rest/api/3/issue/STAT-4583/worklog", new Body { started = started, timeSpent = isEvenNum ? "3h" : "2h" });

        }
    }
}