namespace Playtolia.Entity.Scaffold
{
    /*
     * None("No event induced (no-op)"),
       UICreateTicket("Create a new suppport ticket"),
       UIShowTickets("Display the list of support tickets"),
       UIShowSettings("Display the settings screen"),
       UIDetach("Detach the GameScaffold UI from the screen");
     */
    public enum InducedScaffoldEvent
    {
        None,
        UICreateTicket,
        UIShowTickets,
        UIShowSettings,
        UIDetach
    }
}