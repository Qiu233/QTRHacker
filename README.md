# QTRHacker

**Maybe you have something about this hack to tell,please join this discord server:**  
https://discord.gg/bzKc9vM  

All releases: https://github.com/ZQiu233/QTRHackerUpdatesHistory/tree/master/Updates

Now QTRHacker is much stronger than the old one.
Sorry for that there's no English version for the newest version.
Maybe you should compile it by yourself.

## Environment requests
* Windows 7 or above(32/64).
* .Net Framework 4.6.2 or above.
* VC Runtime(VC Redist).

## How to compile:
1. Install Visual Studio 2017
2. Clone or download this repo.
3. Extract all files if you choice to download.
4. Open the Solution named "QTRHacker" by Visual Studio(Version Selector)
5. Ctrl + F5
6. Then you can see the hack running.

## Core Concepts
QHackLib:A base library for basic hacking in managed and native level.

QTRHacker.Functions:A library base on QHackLib to directly hack Terraria.
QTRHacker.Functions.GameContext:In charge of the most basic information fetching and operations.
QTRHacker.Functions.GameObjects:In charge of game objects such as item,projectile and so on.
QTRHacker.Functions.ProjectileImage:An interface to emmit projectile into game,including rainbow drawing and texting.
QTRHacker.Functions.ProjectileMaker:A compiler for a projectile programming languange.

QTRHacker.NewDimension:A completely new black-themed UI for this hack.

## History(My words)
I began to make this hack in 2015/7,when it was at first named "TR_Hacker".At that time, this hack has nothing more than three simple,useless inf- functions.I had never thought I would keep updating it for so long and I was going to give up in 2016/1.
One day in 2016/6, I met some intersting guys playing TR in a social platform called QQ Group,the leader of whom is making a hack.I asked(even begged) for his hack only to find it was not free to use.It reminded me of my own humble hack which had been shelved for months.After talking to other members about it, I decided to restart this project.
Several days later, I successfully update my hack for the last game version.I modified/optimized some algorithms so that it could be working more efficiently,including AobScan and Inline-Hook.But I also made a wrong decision to turn the original GUI to a black console.
Soon afterwards I completed all functions that the leader's had and published my own hack on the same platform.The respond was not as good as I'd expected.They were not accepting the console because of its inconvenience.
It was also a chance to regenerate the GUI.If I had maintained the GUI from the old version, this hack would not have its later great development.In 2 days, I finished a new GUI for it,whose appearance is not so different while the core is completely new.It was after this time that my hack was becoming famous.I began to keep it secretly used in a small group.
With friends met on QQ, I kept updating for a month and finally made the strongest ever Terraria memory hack in China.Then we began to do something bad to servers.
It was such a time, a lot of servers, a lot of players.I would say there were once more than 20 servers in China in the same time.In such a booming environment, no one could have imagined that a ruinous storm was coming.
In one month, we, about 10 persons, blew up nearly all servers.Some of them was closed at last while some others chose to enable the whitelists, just to defend us.
In 2016/8, for some reason, I decided to found my own organization.I named it "THK", which means "Terraria Hacker".From then on,the updating had formally begun.

As time went by, 2017/6, a man called "ronnin" joined us.He uploaded a vedio to show off this hack.
I never thought there would be so many people interested on it before I watched the number of play growing to 190,000 with my own eyes.It's unbelievable and I decided to publish my own hack.
On 2017/10/1, I published the hack for free.One day later, I founded an outter organization for discussing about it.In a crazy speed, the hack was spread everywhere.I even found someone was reselling it for a profit.

In 2018/8, I decided to rewrite the hack.
It took me more than 2 weeks, but my efforts paid off at last.TR_Hacker was written in a mixed way of C#/C++/C.The main goal of rewriting it is to unitize the programming language.Eventually I remove the part of C++ and C, leaving C#, which was in charge of both GUI and the Core.Yes, the Core and GUI was still separated while the Core is no longger written in C++ or C.
The Core was named "QTRHacker.Functions".As planed,I first made a base library for it named "QHackLib".Then, based on QHackLib, I made QTRHacker.Functions to Terraria.
After rewriting,the GUI was not changed at all, while the core has no common with the old one.Though, there was still one problem to solve,the CheatLibrary, which was made by myself to get CLR information from TR was still there.It was written in C++, and was still essential.I found no way to replace it.

Soon afterwards I found a library called clrmd.It was the one I'd been seeking for so long to replace CheatLibrary.
It worked amazingly well, completely having taken over CheatLibrary's work.
In the same time, I made another new GUI for it.It was much more beautified than before and from then on, the hack's name was changed into "QTRHacker" which is known now.

Well, that's a brief history of QTRHacker.It's too brief to tell the details about the hack.In fact, there were a lot of problems I met during these years.
I think it's unbelievable.It was once such a simple,humble hack but now, as time went by, it has been greatly developed.On the day I began to make it,no one would have thought that all of these things would happened.It's so amazing.

## Screenshots
![](./Screenshots/1.png)
![](./Screenshots/2.png)
![](./Screenshots/3.png)
![](./Screenshots/4.png)
![](./Screenshots/5.png)
![](./Screenshots/6.png)
![](./Screenshots/7.png)
