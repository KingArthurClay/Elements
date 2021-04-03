﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{  

    static Dictionary<QuestName, Quest> quests;

    void Awake()
    {
        QuestManager.quests = new Dictionary<QuestName, Quest>();
        _instantiate_quests();
    }

    void Start()
    {
        //Progress quest if quest phase == 0
        foreach (KeyValuePair<QuestName, Quest> entry in quests)
        {
            Quest quest = entry.Value;
            if (quest.currentPhase == 0)
            {
                EventManager.instance.QuestProgressed(quest.name, 0);
            }
        }
    }

    public static void ProgressQuest( QuestName quest )
    {
        ProgressQuestToPhase(quest, QuestManager.quests[quest].currentPhase + 1);
    }

    public static void ProgressQuestToPhase( QuestName quest, int phase )
    {
        print("Quest: " + quest + "Updated to phase: " + phase);
        QuestManager.quests[quest].currentPhase = phase;
        EventManager.instance.QuestProgressed( quest, quests[quest].currentPhase );
        if ( QuestManager.quests[quest].currentPhase >= QuestManager.quests[quest].totalPhases )
            CompletedQuest(quest);
    }

    public static void CompletedQuest( QuestName completedQuest )
    {
        foreach(KeyValuePair<QuestName, Quest> entry in quests)
        {
            QuestName currName = entry.Key;
            Quest curr = entry.Value;
            if ( curr.currentPhase == -1 && curr.prerequisites.Contains(completedQuest) )
            {
                curr.prerequisites.Remove(completedQuest);
                curr.prereqsCompleted++;

                if ( curr.prerequisites.Count == 0 || curr.prereqsCompleted >= curr.numPrereqsRequired )
                    ProgressQuest(currName);
            }
        }
    }

    public static int GetQuestPhase( QuestName quest )
    {
        return QuestManager.quests[quest].currentPhase;
    }

    public static Dictionary<QuestName, Quest> GetQuests()
    {
        return quests;
    }

    public static List<Quest> GetAvailableQuests()
    {
        List<Quest> availableQuests = new List<Quest>();
        foreach (KeyValuePair<QuestName, Quest> entry in quests)
        {
            Quest curr = entry.Value;
            if (curr.currentPhase >= 0 && curr.currentPhase < curr.totalPhases)
            {
                availableQuests.Add(curr);
            }
        }

        return availableQuests;
    }

    private void _instantiate_quests()
    {
        // All quests have to be added to QuestName and then added here
        QuestManager.quests.Add( 
            QuestName.PerformDiagnostics, 
            new Quest( 
                QuestName.PerformDiagnostics,
                new List<QuestName>{},
                new List<( string hint, string image )> {
                    ( "Automatic", null ),
                    ( "Turn on the first portal by hitting the button", "Images/test" ),
                    ( "Go through the newly active portal and collect your weapon", "Images/test" ),
                    ( "Return through the portal and destroy the box containing your tools", "Images/test" ),
                    ( "Use your tools to fix the generator" , "Images/test" )
                }) 
        );

        QuestManager.quests.Add( 
            QuestName.GettingALayOfTheLand, 
            new Quest( 
                QuestName.GettingALayOfTheLand, 
                new List<QuestName>{ QuestName.PerformDiagnostics },
                new List<( string hint, string image )> {
                    ( "Automatic", null ),
                    ( "Talk to the Bank Manager at Morgan's Panley", "Images/test" ),
                    ( "Talk to the Shogun of Bakufu", "Images/test" ),
                    ( "Talk to the Chief of Fimbultoft", "Images/test" ),
                })
        );

        QuestManager.quests.Add( 
            QuestName.ForGlory, 
            new Quest( 
                QuestName.ForGlory, 
                new List<QuestName>{ QuestName.PerformDiagnostics },
                new List<( string hint, string image )> {
                    ( "Talk to the Shogun of Bakufu", "Images/test" ),
                    ( "Follow the Shogun", "Images/test" ),
                    ( "Step into the arena", "Images/test" ),
                    ( "Prepare to fight", "Images/test" ),
                    ( "Defeat Helga", "Images/test" ),
                    ( "Defeat Musashi The Machine","Images/test" ),
                    ( "Defeat Stacy From Accounting", "Images/test" ),
                    ( "Talk to the shogun (automatic)", "Images/test" )
                })
        );

        QuestManager.quests.Add( 
            QuestName.WhateverFloatsYourBoat, 
            new Quest( 
                QuestName.WhateverFloatsYourBoat, 
                new List<QuestName>{ QuestName.PerformDiagnostics },
                new List<( string hint, string image )> {
                    ( "Talk to Viking Chief", "Images/test" ),
                    ( "Talk to Sven at the Docks", "Images/test" ),
                    ( "Talk to Ormen at the Lumberyard", "Images/test" ),
                    ( "Take a Stack of Planks back to Sven","Images/test" ),
                    ( "Listen to Sven", "Images/test" ),
                    ( "Find Sigrid in her home", "Images/test" ),
                    ( "Take the sail back to Sven", "Images/test" ),
                    ( "Listen to Sven", "Images/test" ),
                    ( "Talk to the Viking Chief", "Images/test" )
                })
        );

        QuestManager.quests.Add( 
            QuestName.AnIdiotsLuggage, 
            new Quest( 
                QuestName.AnIdiotsLuggage, 
                new List<QuestName>{ QuestName.PerformDiagnostics },
                new List<( string hint, string image )> {
                    ( "Talk to the Bank Manager at Morgan's Panley", "Images/test" ),
                    ( "Speak with Amanda the Teller", "Images/test" ),
                    ( "Speak with Damien in Security", "Images/test" ),
                    ( "Speak with Olive, the manager's assistant", "Images/test" ),
                    ( "Guess the Manager's Password (hints: green, benjamin, bills)", "Images/test" ),
                    ( "Speak to the manager (automatic)", "Images/test" )
                }) 
        );

        QuestManager.quests.Add( 
            QuestName.CarlTheJarl,
            new Quest(
                QuestName.CarlTheJarl,
                new List<QuestName>{ QuestName.PerformDiagnostics },
                new List<( string hint, string image )> {
                    ( "Talk to the Chief of Fimbultoft", "Images/test" ),
                    ( "Go to Fimbultoft; Find Carl (maybe ask villager to tell you where he is)", "Images/test" ),
                    ( "Find Carl's home-made shield in the Fimbulwald (the forest)", "Images/test" ),
                    ( "Return Carl's Shield to him", "Images/test" ),
                    ( "Return to the bank manager", "Images/test" )
                }) 
        );

        QuestManager.quests.Add( 
            QuestName.GoldenHandcuffs, 
            new Quest( 
                QuestName.GoldenHandcuffs, 
                new List<QuestName>{ QuestName.AnIdiotsLuggage },
                new List<( string hint, string image )> {
                    ( "Talk to the Bank Manager at Morgan's Panley", "Images/test" )
                }) 
        );

        QuestManager.quests.Add( 
            QuestName.ButThisIsMyBirthCertificate, 
            new Quest( 
                QuestName.ButThisIsMyBirthCertificate, 
                new List<QuestName>{ QuestName.ForGlory },
                new List<( string hint, string image )> {
                    ( "Talk to the Bank Manager at Morgan's Panley", "Images/test" )
                }) 
        );
        
        QuestManager.quests.Add( 
            QuestName.InternLunchHour, 
            new Quest( 
                QuestName.InternLunchHour, 
                new List<QuestName>{ QuestName.AnIdiotsLuggage },
                new List<( string hint, string image )> {
                    ( "Talk to the Bank Manager at Morgan's Panley", "Images/test" )
                }) 
        );

        QuestManager.quests.Add( 
            QuestName.CalculatorCatastrophe, 
            new Quest( 
                QuestName.CalculatorCatastrophe, 
                new List<QuestName>{ QuestName.AnIdiotsLuggage, QuestName.ForGlory },
                new List<( string hint, string image )> {
                    ( "Talk to the Bank Manager at Morgan's Panley", "Images/test" )
                }) 
        );

        QuestManager.quests.Add( 
            QuestName.ThisIsPureSnow, 
            new Quest( 
                QuestName.ThisIsPureSnow, 
                new List<QuestName>{ QuestName.AnIdiotsLuggage, QuestName.CarlTheJarl },
                new List<( string hint, string image )> {
                    ( "Talk to the Bank Manager at Morgan's Panley", "Images/test" )
                }) 
        );

        QuestManager.quests.Add( 
            QuestName.ThingsAreGettingSerious, 
            new Quest( 
                QuestName.ThingsAreGettingSerious, 
                new List<QuestName>{ QuestName.GoldenHandcuffs, QuestName.ButThisIsMyBirthCertificate, QuestName.InternLunchHour, QuestName.CalculatorCatastrophe, QuestName.ThisIsPureSnow },
                new List<( string hint, string image )> {
                    ( "Talk to the Bank Manager at Morgan's Panley", "Images/test" )
                },
                3 )
        );

        QuestManager.quests.Add( 
            QuestName.TheGreatWolf, 
            new Quest( 
                QuestName.TheGreatWolf, 
                new List<QuestName>{ QuestName.ThingsAreGettingSerious },
                new List<( string hint, string image )> {
                    ( "Talk to the Bank Manager at Morgan's Panley", "Images/test" )
                })
        );
        
        QuestManager.quests.Add( 
            QuestName.BjornAndMoose, 
            new Quest( 
                QuestName.BjornAndMoose, 
                new List<QuestName>{ QuestName.ThingsAreGettingSerious },
                new List<( string hint, string image )> {
                    ( "Talk to the Bank Manager at Morgan's Panley", "Images/test" )
                })
        );

        QuestManager.quests.Add( 
            QuestName.ForbiddenLovers, 
            new Quest( 
                QuestName.ForbiddenLovers, 
                new List<QuestName>{ QuestName.ThingsAreGettingSerious },
                new List<( string hint, string image )> {
                    ( "Talk to the Bank Manager at Morgan's Panley", "Images/test" )
                })
        );

        QuestManager.quests.Add( 
            QuestName.ForTheUnion, 
            new Quest( 
                QuestName.ForTheUnion, 
                new List<QuestName>{ QuestName.ThingsAreGettingSerious },
                new List<( string hint, string image )> {
                    ( "Talk to the Bank Manager at Morgan's Panley", "Images/test" )
                })
        );

        QuestManager.quests.Add( 
            QuestName.TheEnd, 
            new Quest( 
                QuestName.TheEnd, 
                new List<QuestName>{ QuestName.TheGreatWolf, QuestName.BjornAndMoose, QuestName.ForbiddenLovers, QuestName.ForTheUnion },
                new List<( string hint, string image )> {
                    ( "Talk to the Bank Manager at Morgan's Panley", "Images/test" )
                },
                3 )
        );
    
    
    }

    public static void LoadQuestPhases(PlayerData data)
    {
        int i = 0;
        foreach (KeyValuePair<QuestName, Quest> entry in quests)
        {
            QuestName questName = entry.Key;
            Quest quest = entry.Value;

            //May be called before Start function, so progress quests to 0 if necessary
            if (quest.currentPhase == 0)
            {
                EventManager.instance.QuestProgressed(quest.name, 0);
            }

            //Activate all QuestPhaseListeners (Iterate through each quest phase)
            for (int questPhase = quest.currentPhase; questPhase < data.GetQuestPhase(i); ++questPhase)
            {
                ProgressQuest(questName);
            }

            ++i;
        }
    }
}
