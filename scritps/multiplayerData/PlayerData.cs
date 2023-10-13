public class PlayerData {
    public string name;
    public long id;
    public int score;

    public PlayerData(string name, long id, int score = 0) {
        this.name =name;
        this.id =id;
        this.score =score;
    }
}