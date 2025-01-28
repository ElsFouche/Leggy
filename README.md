Leggy the Miraculous Robotic Arm

main is the backup. Don’t futz with it except to update with stable, functioning code. Updates to main require review first. 

dev is the main working branch. Once stuff is tested and functioning properly and we know it’s not going to break things we can move it to main. 

dev and main will be in sync only when we’re creating a backup of the current dev branch by merging it to main. 

This means you need to branch from dev when you’re working on something, not from main. 

Branch from dev when you’re working on a feature. Complete the feature, test it, then merge it back to dev.

Steps to working on a feature:
 - Create a branch from dev. 
 - Name it something descriptive. Work on one thing at a time.
 - Work on it. Test it.
 - When it's ready, create a pull request from your branch to dev.
 - Repeat.

When we're at a position where things are stable and functioning well, we create a pull request from dev to main.
