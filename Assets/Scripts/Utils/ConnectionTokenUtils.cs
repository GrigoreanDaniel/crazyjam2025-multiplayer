using System;

public static class ConnectionTokenUtils
{
    /// <summary>
    /// Create new random token
    /// </summary>
    public static byte[] Newtoken() => Guid.NewGuid().ToByteArray(); // Generate a new token using a GUID

    /// <summary>
    /// Converts a token into a Hash format
    /// </summary>
    /// <param name="token">Token to be hashed</param>
    public static int HashToken(byte[] token) => new Guid(token).GetHashCode(); // Convert the token to an integer hash

    /// <summary>
    ///  Converts a token into a string
    /// </summary>
    /// <param name="token"> Token to be parsed</param>
    public static string TokenToString(byte[] token) => System.Convert.ToBase64String(token); // Convert the token to a Base64 string
}
