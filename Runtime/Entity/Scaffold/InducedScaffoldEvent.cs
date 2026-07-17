namespace Playtolia.Entity.Scaffold
{
    /*
     * None("No event induced (no-op)"),
       UIShowMenu("Display the main overlay menu"),
       UICreateTicket("Create a new suppport ticket"),
       UIShowTickets("Display the list of support tickets"),
       UIShowSettings("Display the settings screen"),
       UIShowFriends("Display the friends list screen"),
       UIShowProfile("Display the profile screen"),
       UIDetach("Detach the GameScaffold UI from the screen");
     */
    // Marshalled to Kotlin by name via ToString(), so these must match
    // com.playtolia.sdk.ui.scaffold.event.InducedScaffoldEvent exactly.
    public enum InducedScaffoldEvent
    {
        None,
        UIShowMenu,
        UICreateTicket,
        UIShowTickets,
        UIShowSettings,
        UIShowFriends,
        UIShowProfile,
        UIDetach
    }
}
