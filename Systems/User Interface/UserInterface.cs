using System.Collections.Generic;
using UnityEngine;

public class UserInterface : MonoBehaviour
{
    public static UserInterface Instance;

    [SerializeField] private UserInterface_Layout none;
    [SerializeField] private UserInterface_Layout mainMenu;
    [SerializeField] private UserInterface_Layout pauseMenu;
    [SerializeField] private UserInterface_Layout inGame_Default;
    [SerializeField] private UserInterface_Layout inGame_Opening;
    [SerializeField] private UserInterface_Layout inGame_OpeningWithObjective;
    [SerializeField] private UserInterface_Layout inGame_OpeningWithJournal;
    [SerializeField] private UserInterface_Layout inGame_Credits;

    [SerializeField] private UserInterface_LayoutDataInjector userInterface_LayoutDataInjector;

    [SerializeField] private TrophyAnimationTargets trophyAnimationTargets;

    private UserInterfaceLayout activeUserInterfaceLayoutType = UserInterfaceLayout.Uninitialized;
    private UserInterface_Layout activeUserInterfaceLayout = null;

    private List<UserInterface_Layout> layoutList;

    public UserInterfaceLayout ActiveUserInterfaceLayoutType => activeUserInterfaceLayoutType;
    public TrophyAnimationTargets TrophyAnimationTargets => trophyAnimationTargets;

    public void ChangeUserInterfaceLayout(UserInterfaceLayout userInterfaceLayout)
    {
        SwitchLayout(userInterfaceLayout);
    }

    private void SwitchLayout(UserInterfaceLayout userInterfaceLayout, bool forceUpdate = false)
    {
        if (userInterfaceLayout == activeUserInterfaceLayoutType
            && forceUpdate == false)
        {
            return;
        }

        if (activeUserInterfaceLayout != null)
        {
            activeUserInterfaceLayout.DeactivateLayout();
        }

        switch (userInterfaceLayout)
        {
            case UserInterfaceLayout.None:
                activeUserInterfaceLayout = none;
                break;
            case UserInterfaceLayout.MainMenu:
                activeUserInterfaceLayout = mainMenu;
                break;
            case UserInterfaceLayout.PauseMenu:
                activeUserInterfaceLayout = pauseMenu;
                break;
            case UserInterfaceLayout.InGame_Default:
                activeUserInterfaceLayout = inGame_Default;
                break;
            case UserInterfaceLayout.InGame_Opening:
                activeUserInterfaceLayout = inGame_Opening;
                break;
            case UserInterfaceLayout.InGame_Credits:
                activeUserInterfaceLayout = inGame_Credits;
                break;
            case UserInterfaceLayout.InGame_OpeningWithJournal:
                activeUserInterfaceLayout = inGame_OpeningWithJournal;
                break;
            case UserInterfaceLayout.InGame_OpeningWithObjective:
                activeUserInterfaceLayout = inGame_OpeningWithObjective;
                break;
            default:
                activeUserInterfaceLayout = none;
                DebugWrapper.LogWarning("Unhandled UserInterfaceLayout - UserInterface.SwitchLayout(...)", gameObject);
                break;
        }

        activeUserInterfaceLayoutType = userInterfaceLayout;
        activeUserInterfaceLayout.ActivateLayout();
    }

    private void InjectUserInterfaceLayoutData()
    {
        foreach (UserInterface_Layout layout in layoutList)
        {
            userInterface_LayoutDataInjector.InjectDataTo(layout);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }

        layoutList = new List<UserInterface_Layout>() { none, mainMenu, pauseMenu, inGame_Default, inGame_Opening, inGame_Credits, inGame_OpeningWithJournal, inGame_OpeningWithObjective };
        InjectUserInterfaceLayoutData();
        ChangeUserInterfaceLayout(UserInterfaceLayout.None);
    }
}
