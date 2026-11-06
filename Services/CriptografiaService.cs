
using System.Security.Cryptography;
using System.Text;

namespace ClinicaOdontologica.Services;

public class CriptografiaService
{
    private readonly byte[] chave;

    public CriptografiaService(string chaveSecreta)
    {
        chave = SHA256.HashData(
            Encoding.UTF8.GetBytes(chaveSecreta)
        );
    }

    public string Criptografar(string texto)
    {
        using var aes = Aes.Create();

        aes.Key = chave;
        aes.GenerateIV();

        using var memoria = new MemoryStream();

        memoria.Write(aes.IV, 0, aes.IV.Length);

        using (var criptografia = new CryptoStream(
            memoria,
            aes.CreateEncryptor(),
            CryptoStreamMode.Write))
        using (var escritor = new StreamWriter(criptografia))
        {
            escritor.Write(texto);
        }

        return Convert.ToBase64String(memoria.ToArray());
    }

    public string Descriptografar(string textoCriptografado)
    {
        byte[] dados = Convert.FromBase64String(
            textoCriptografado
        );

        using var aes = Aes.Create();

        aes.Key = chave;

        byte[] iv = dados[..16];

        using var memoria = new MemoryStream(dados, 16, dados.Length - 16);

        aes.IV = iv;

        using var descriptografia = new CryptoStream(
            memoria,
            aes.CreateDecryptor(),
            CryptoStreamMode.Read
        );

        using var leitor = new StreamReader(descriptografia);

        return leitor.ReadToEnd();
    }
}
