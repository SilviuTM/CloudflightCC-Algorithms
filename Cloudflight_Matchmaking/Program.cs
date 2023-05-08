# region Read and Ready
string? level = Console.ReadLine();
string input = File.ReadAllText(Directory.GetCurrentDirectory() + "/../../../input/level" + level + ".in");
input = input.Replace("\r\n", " ");
input = input.Replace("\n", " ");
input = input.Trim();

string[] data =  input.Split(' ');

int games = int.Parse(data[0]);
int players  = int.Parse(data[1]);
int TEAM_SIZE = int.Parse(data[2]);

int Kfactor = 32;

Player[] player = new Player[players]; for (int i = 0; i < player.Length; i++) player[i] = new Player(i, 1000);
# endregion

# region Initial Skirmishes
int index = 3, gameno = 0;
while (gameno < games)
{
    gameno++;
    int[] team1ID = new int[TEAM_SIZE];
    int[] team2ID = new int[TEAM_SIZE];

    int[] team1score = new int[TEAM_SIZE];
    int[] team2score = new int[TEAM_SIZE];

    double[] team1eloToChange = new double[TEAM_SIZE];
    double[] team2eloToChange = new double[TEAM_SIZE];

    for (int mate = 0; mate < TEAM_SIZE; mate++)
    {
        team1ID[mate] = int.Parse(data[index]);
        team1score[mate] = int.Parse(data[index + 1]);

        index += 2;
    }

    for (int mate = 0; mate < TEAM_SIZE; mate++)
    {
        team2ID[mate] = int.Parse(data[index]);
        team2score[mate] = int.Parse(data[index + 1]);

        index += 2;
    }

    double EloScoreA = 0f;
    double EloScoreB = 0f;
    if (team1score.Sum() > team2score.Sum())
    {
        EloScoreA = 1f;
    }
    if (team1score.Sum() < team2score.Sum())
    {
        EloScoreB = 1f;
    }

    // compute ELO change
    for (int mate = 0; mate < TEAM_SIZE; mate++)
    {
        double Expect = 1f / (1f + Math.Pow(10f, (ComputeEnemyTeamElo(team2ID) - ComputeOwnTeamElo(team1ID, team1ID[mate]) 
                                                            - player[team1ID[mate]].elo) / 400f));
        
        team1eloToChange[mate] = player[team1ID[mate]].elo + (Kfactor * (EloScoreA - Expect));
    }

    for (int mate = 0; mate < TEAM_SIZE; mate++)
    {
        double Expect = 1f / (1f + Math.Pow(10f, (ComputeEnemyTeamElo(team1ID) - ComputeOwnTeamElo(team2ID, team2ID[mate])
                                                            - player[team2ID[mate]].elo) / 400f));
        
        team2eloToChange[mate] = player[team2ID[mate]].elo + (Kfactor * (EloScoreB - Expect));
    }

    // actually change elo
    for (int mate = 0; mate < TEAM_SIZE; mate++)
        player[team1ID[mate]].elo = (int)team1eloToChange[mate];

    for (int mate = 0; mate < TEAM_SIZE; mate++)
        player[team2ID[mate]].elo = (int)team2eloToChange[mate];
}
# endregion

int maxEloDiff = int.Parse(data[index++]);
int scoreThresh = int.Parse(data[index++]);
int rePlayers = int.Parse(data[index++]);

# region Queue Regoers and Sort them
List<Player> rePlayer = new List<Player>();
while (index < data.Length)
{
    int reID = int.Parse(data[index++]);
    for (int i = 0; i < players; i++)
        if (player[i].id == reID)
        {
            rePlayer.Add(player[i]);
            i = 9999999;
        }
}

for (int i = 0; i < rePlayer.Count - 1; i++)
    for (int j = i + 1; j < rePlayer.Count; j++)
        if (rePlayer[i].elo < rePlayer[j].elo)
            (rePlayer[j], rePlayer[i]) = (rePlayer[i], rePlayer[j]);

        else if (rePlayer[i].elo == rePlayer[j].elo)
            if (rePlayer[i].id > rePlayer[j].id)
                (rePlayer[j], rePlayer[i]) = (rePlayer[i], rePlayer[j]);
# endregion

# region Level 6 Tentative (backtrack not fine-tuned)
StreamWriter sw = new(Directory.GetCurrentDirectory() + "/../../../output/level" + level + ".out", false);

int k = 0;
int[] xteam1ID = new int[TEAM_SIZE];
int[] xteam2ID = new int[TEAM_SIZE];
while (rePlayer.Count > 0)
{
    xteam1ID = new int[TEAM_SIZE];
    xteam1ID[0] = rePlayer[0].id;

    k = 0;
    while (k + 1 < rePlayer.Count && rePlayer[0].elo - rePlayer[k + 1].elo <= maxEloDiff) k++;
    for (int j = 0; j < TEAM_SIZE - 1; j++)
        xteam1ID[j + 1] = rePlayer[k - j].id;
    k = k - (TEAM_SIZE - 2); // k excluded from available

    backtrack(new List<int>());

    foreach (var guy in xteam1ID)
        sw.Write(guy + " ");
    foreach (var guy in xteam2ID)
        sw.Write(guy + " ");
    sw.Write("\n");

    foreach (var guy in xteam1ID)
        for (int i = 0; i < rePlayer.Count; i++)
        {
            if (rePlayer[i].id == guy)
            {
                rePlayer.RemoveAt(i);
                i--;
            }
        }
    foreach (var guy in xteam2ID)
        for (int i = 0; i < rePlayer.Count; i++)
        {
            if (rePlayer[i].id == guy)
            {
                rePlayer.RemoveAt(i);
                i--;
            }
        }
}

sw.Close();

void backtrack(List<int> team)
{
    if (team.Sum() == TEAM_SIZE || team.Count >= k)
    {
        if (team.Sum() == TEAM_SIZE && team.Count < k) 
        {
            int elosum = 0;
            for (int i = 0; i < team.Count; i++)
                if (team[i] == 1)
                    elosum += (int)rePlayer[i + 1].elo;

            if (Math.Abs(ComputeOwnTeamElo(xteam1ID, -1) - elosum) <= scoreThresh) 
            {
                int kounter = 0;
                for (int i = 0; i < team.Count; i++)
                    if (team[i] == 1)
                        xteam2ID[kounter++] = rePlayer[i + 1].id;
            }
        }
        return;
    }

    List<int> newteam = new(team);
    newteam.Add(0);
    backtrack(newteam);
    newteam[^1] = 1;
    backtrack(newteam);
}

# endregion

int ComputeOwnTeamElo(int[] mates, int you)
{
    int totalelo = 0;
    foreach (int mate in mates)
    {
        foreach (Player p in player)
            if (p.id == mate && mate != you)
            {
                totalelo += (int)p.elo;
            }
    }

    return totalelo;
}

int ComputeEnemyTeamElo(int[] enemies)
{
    int totalelo = 0;
    foreach (int enemy in enemies)
    {
        foreach (Player p in player)
            if (p.id == enemy)
            {
                totalelo += (int)p.elo;
            }
    }

    return totalelo;
}

struct Player
{
    public int id;
    public double elo;

    public Player(int id, double elo)
    {
        this.id = id;
        this.elo = elo;
    }
}