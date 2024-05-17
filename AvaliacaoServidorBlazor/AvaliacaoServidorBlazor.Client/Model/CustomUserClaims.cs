namespace AvaliacaoServidorBlazor.Client.Model
{
    public class CustomUserClaims
    {
        public CustomUserClaims()
        {
        }

        public CustomUserClaims(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
