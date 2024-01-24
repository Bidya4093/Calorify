using System.Collections.Generic;

public class ActivitiesManager
{
    public List<Activity> activitiesList;

    public ActivitiesManager() 
    {
        var dataService = new DataService("activitiesdb.db");
        var activities = dataService.GetConnection().Table<Activity>();
        activitiesList = new List<Activity>();
        ToList(activities);
    }

    // returns list of activities that include given substring in its name
    public List<Activity> IncludeSubstring(string substr) 
    {
        List<Activity> returnList = new List<Activity>();
        foreach(Activity activity in activitiesList)
        {
            if(activity.activity_name.ToLower().Contains(substr.ToLower())) returnList.Add(activity);
        }
        return returnList;
    }

    // returns activity by its id. If not found - returns null
    public Activity GetById(int id)
    {
        foreach (Activity activity in activitiesList)
        {
            if (activity.activity_id == id) return activity;
        }
        return null;
    }

    private void ToList(IEnumerable<Activity> activities)
    {
        foreach (var activity in activities)
        {
            activitiesList.Add(activity);
        }
    }
}
