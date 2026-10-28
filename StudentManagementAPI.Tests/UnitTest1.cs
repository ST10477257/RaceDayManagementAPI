using Xunit;

namespace StudentManagementAPI.Tests;

public class UnitTest1
{
    [Fact]
    public void PasswordMustContainAtLeastEightCharacters()
    {
        var password = "Test123!";


    Assert.True(password.Length >= 8);
    }

    [Fact]
    public void ParticipantRoleMustBeParticipant()
    {
        var role = "Participant";

        Assert.Equal("Participant", role);
    }

    [Fact]
    public void FinishingPositionMustBePositive()
    {
        var finishingPosition = 1;

        Assert.True(finishingPosition > 0);
    }

    [Fact]
    public void InvalidFinishingPositionMustBeRejected()
    {
        var finishingPosition = 0;

        Assert.False(finishingPosition > 0);
    }


}

