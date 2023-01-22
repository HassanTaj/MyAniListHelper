
# Learn .Net MAUI

## With Code Challenge

----

### Problem

 As an everyday Anilist user there are some problems i am facing while adding new anime to my Anilist profile, because first i have to maintain a local list from where I move all the stored anime names to Anilist using my desktop browser.  
It takes a lot of time and it gets frustrating when after so many clicks and key strokes I find out that I've already watched it or it is completed.

### Solution

The solution is as follows:

**Features:**

- Login User
- Get all the user data and sync it into App db
- Provide a screen for search where user can
- Enter a term and search for existing anime
- If it's not found provide a button to search for the anime from Anilist
- The search result than should show a tile that helps user add the anime to Anilist profile with 1 click.

**Challenging Features:**

- Add settings page
  - Allow user to control sync time and occurrence
  - Allow user to view previously available anime that were previously synced from Anilist
  - Allow user to Logout
  
### Getting Started

- Install Visual Studio
- Install .Net MAUI Package
- Clone the repository
- Create a branch from `dev` for yourself `dev_{your_name_here}`
- Commit your code only into your branch
- Build project
- Run the project by pressing F5

**For Emulator:**

- Create a Virtual Device using AVD
- Run the virtual device
- Pres F5 and run the project  

**For Physical Device:**

- Enable developer mode in your device
- **Wired**
  - Connect your device using a wire
  - Allow debugging on your phone when you see the prompt
- **Wireless**
  - Find enable WiFi debugging in developer mode page
  - Enable WiFi debugging and note the IP address provided
  - Use `adb --connect xxx.xxx.xx.xx:port` to connect to your device
  - User `adb devices` to check if your devices is connected
  - Allow when prompted
  - Press F5 to enter debugging mode

----

### Challenge

**What's Done:**

- [x] Log In
- [x] Get user data from Anilist
- [x] Sync Data in local db
- [x] Search and filter local data
- [x] Search for anime on Anilist (if not present locally)

**What needs to be done:**

- [ ] Item list-view change from text to tiles and images
- [ ] Give an item detail view page from where user can add the anime into Anilist
- [ ] Sync the newly added anime in local db once it's added to Anilist database
- [ ] Logout user on demand
- [ ] Better UI Overall

**Challenging Features:**

- [ ] Allow user to control sync time and occurrence
- [ ] Allow user to view previously available anime that were previously synced from Anilist
- [ ] Allow user to Logout

**Reminder!**
This is the test for your true courage and your change to prove how driven you are.
> Never let anyone take away a learning opportunity, because every missed learning opportunity is a potential regret in the future, ― Lee Haisen

``P.S``  Don't let me take away your learning opportunity, and **best of luck.**
