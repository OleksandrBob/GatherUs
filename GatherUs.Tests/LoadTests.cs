using System.Text;
using NBomber.CSharp;
using NBomber.Http.CSharp;

namespace GatherUs.Tests;

public static class LoadTests
{
    private const string UserToken =
        "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjkiLCJuYW1laWQiOiI5IiwiRW1haWwiOiJ6b3lkaXpva25hQGd1ZnVtLmNvbSIsIk5hbWUiOiJBbGV4IE1pbHRvbiIsInJvbGUiOiJPcmdhbml6ZXIiLCJuYmYiOjE3MTU2MDk3MjgsImV4cCI6MTcxNTY0MzAyOCwiaWF0IjoxNzE1NjA5NzI4LCJpc3MiOiJHYXRoZXJVc1NlcnZlciIsImF1ZCI6IkdhdGhlclVzQ2xpZW50In0.7pOcloQJTepLx4pRmR2G4ATf_LS3IovJ_qKtdOJK5jc";

    public static void GetCurrentUserInfoTest()
    {
        using var httpClient = new HttpClient();

        var scenario = Scenario.Create("http_getCurrentUser", async context =>
        {
            var request =
                Http.CreateRequest("GET", "https://localhost:7172/api/events/currentUser")
                    .WithHeader("Accept", "application/json")
                    .WithHeader("Authorization",
                        UserToken);

            var response = await Http.Send(httpClient, request);

            return response;
        });

        NBomberRunner
            .RegisterScenarios(scenario)
            .Run();
    }

    public static void SignInTest()
    {
        using var httpClient = new HttpClient();

        var scenario = Scenario.Create("http_signInUser", async context =>
        {
            var request =
                Http.CreateRequest("POST", "https://localhost:7172/api/Accounts/sign-in")
                    .WithHeader("Accept", "application/json")
                    .WithBody(new StringContent(
                        "{\n  \"mail\": \"nalmobepso@gufum.com\",\n  \"password\": \"1234567890Qwe@\"\n}",
                        Encoding.UTF8, "application/json"));

            var response = await Http.Send(httpClient, request);

            return response;
        });

        NBomberRunner
            .RegisterScenarios(scenario)
            .Run();
    }

    public static void UploadUserPicturePictureTest()
    {
        using var httpClient = new HttpClient();

        var scenario = Scenario.Create("http_updatePicture", async context =>
        {
            var request =
                Http.CreateRequest("PUT", "https://localhost:7172/api/users/current/picture")
                    .WithHeader("Accept", "application/json")
                    .WithHeader("Authorization",
                        UserToken)
                    .WithBody(new StringContent(
                        "{\n  \"picture\": \"iVBORw0KGgoAAAANSUhEUgAAAQAAAAEACAYAAABccqhmAAAABGdBTUEAALGPC/xhBQAAACBjSFJNAAB6JgAAgIQAAPoAAACA6AAAdTAAAOpgAAA6mAAAF3CculE8AAAAB3RJTUUH5QQcFQ4Hfxi3wAAAAAZiS0dEAAAAAAAA+UO7fwAAKfRJREFUeNrtfQmUG9WVtmZIzuEcArQdL2Dwhrf23t02ZjFgIHjBNpvxjhcIYDbveMcEQ8LMEGCYYQgkTEImYSYBJkMCJttACH+U5QTIRsL/h9WE1eBWd2tXt5b731tVUlerVaX3pJL0quq+c66/7lf3fXpf3UVPUrs7ECgzOg8EAqG9nzmqfe/xR3fsOWZ4aPexq0N7jjvQvuf4p9B+wcbG1nB7imqSapNqlGqVapZqt6qBpJ8O3XLM0NDeY1cg6dNoITRgY2NT1qhGn6aaDe07Zkho93Gfki78zj1H/1373mOPbd9z3CIkexYtxTeWjc1VlsKTwUGs4blUy1TTQsV/ZA8e9/d8ZkT73uP2IslHfCPZ2FxtH1Etd+z9zEmd+475e/tnfnTAbjEGF3wFFyb55rGxecKSob3H3dOx79hRlk2AjgjUJciRbxgbm/dMawJ0Eij1cqB9t/aafy8/87OxefckoNU41nrR6/7jPn1kz/GL2vcYr/n3GAvMWGquGlSVi7WyVm9r/ah99/HzQruMTwc6twYCoZ3HDMULzxUWVmJ7m/qiE1YLLic4WStrdbPW3cc/gzU/hGo/cGT3cUe17z5uBV5IObZRNjY2lS1FNU+1jw3g+KPRnm7f3USdAazR7lolqCoXa2Wt3tdKNU+1H+jY3TQcJ0K9m6jMQkXoJFctOFkra/WzVqp5qv1AaFfTFe27jO6w63h5ouK11XCV43SSi7WyVp9rpdoPtO9quh2/wIkmyKOMhQTRzVyslbV6VOvtgSM7m55u32k4FKOd7ZTEenF5bX+slfOuRlxU+9QAgqFdA7SJYuxHbpqzWlMOCzyqcrFW1uoTrVT7WgNo30FOeCGPO0uhxbUdZdBqXa25quJUQOsO1upKrTvdo1VvADvwBGBMWGFhgzY+oljYhANcIROXo/tjrazVB1qp9gP4jXYCCO0wLligiI8MqsrFWlmrX7RS7esnAG0iv4nSKOIjg6pysVbW6het+glgO54AbsYJNDsU8ZFBVblYK2v1i1aq/cAR/MftwWj3UeKxVtbqlFaqff0EsN24UAq362j+2tJXFBXmYq2s1S9a9RPANjwBbB9oLLBGER8ZVJWLtbJWv2il2g+0bxsQRNMvFGHIjEVz7ZJYvL4arnYbLkf2x1pZqw+0Uu1jA8ATwLb8IjsU8ZFBVblYK2v1h9b2wglgqzFhYGjrwP5Yas60xgqt1lXCZcm5zeH9sVbW6gOt+glg64Bgn0WVmt1GvMLFWlmrh7RS7esNYGv1hO1F6CSnqlx+2F87x8KzWvUGsMWZBsDGxuYuo9oPtG/GBrAFJ6q2zxahk5yqcQ1kray1gVzOcFLtYwMYGAxtLkVe6sFsfDYXYTl/Ga7N9eSqQOtm1spaa8i1pTZaqfYD7Zs+iw2AJtjY2PxkVPt6A9g0UJ/Mo4gVrymHTnLWm0v1/bFWf2utkLO3AWgTeQcLFPGRQVW5WCtr9YnW3gaw0bSJSm1jETrJpfr+WCtrdaFWvQFspJcAgwxSKzQ/8KAq0eDZ6ASnk1yslbX6SyvVvt4ANpo2weYb69w/RTO+F/60QgPo2Kh3BSs0WzlfUVSZyw9aO3aMhORP7tWMvua4+k+r3gBuHBQM3YTON+EFWxTxkUFVufygdRCE77sI0m/9Fnpe+wWE757vKDfH1R1aqfa1BtBhbMAKzdbhEKrM5XmtW4dD/PsHANIpzehrmuO4+ktroQGEbjS6Qim8cVCvWflIodk+Wz2nk/vzidbwl+fhM38Q8oO+pjnPxtXx/XlDq94AbsATgOFshWbrqBJDDnKGarw/L2rt2DYSEt+7VX/2zw/8muboGsfVP1qp9rUGELrBWGCHIj4yqCqXl7XeOBjCd82F9Ou/hOJBc3SNfDiu/tCqNYAQnQBuGKxN2KGIjwyqyuVprdtGQ/yJW/o++5tOAXSNfDiuvtGqN4DQ9caEDYr4yKCqXN7VOgTC/3A+pN/4DVgNukY+5Mtx9b5WvQFcb5wAzA7X98dSc0Ibslhnd60uXD7T2rGVnv33YZV3g3UH6NZ8yJfj6gutegPodRhsiXbXSj+Yvb8c16AquOT35zWt2rP/F+dA+q0XodwgH/KlNRxXb2vVG8B1eAKoWqw8qsrlSa1b8Nn/cXz2z3SXbQDkQ760huPqca3XUQPYgA3gOvoGJ21QxEcGVeXymtaO64dA14HZkD70OxAd5EtraC3H1cNaNxgNILTBgQ1uKEJHNlgDLr9p3TTSePbvEW4A5KudAnAtx9W7WvUGcC2eADYM0Uk3DLZE62tDymDpdaGKOGvP1VHmmqu0XofP/vtnQebdV0B20BpaSxwcV1W1DqlO67VaAxgcRDMmrB/EcgPXlkGLdRVxXSvBVcX+vKK148bh+Ey+H6s5DfIdIK2tJQ6Oqze1Uu0HOowTQEc+aaxQxEcGVeXyila0rt0tkPnwr1DpoLXEIXVPOK4u0jqIGoBxArjW6A4m7OiDfedCktjRDyvnCtlwObM/D2i97kSIP/EFgGym4gZAa4mDuDiuXtRKJ4BrBgc1x2uMTVihiI8MqsrlEa2d2yZB9pNDUO0gDuLiuHpRq9EAQsYGrNBsoSqxVlyq7y9UZ874fx8AyGWrbgDEQVwqa+W8q5QrfwKwEcbmPuvcOAayHR+AU4O4iJPvrdeMGsDn8QRwNX6DZoV21ypFP3A1an/xx29z5tnfdAqIfWs7x8JrWj9vNICOq4caC+xQxEcGVeVyudZrh0E29D44PTLv/1Xj5rh6SKveAIYGOz6fn7BCw2x9RNFsQ6rndJLLca1D6q41/sgWyPWkHG8AxEncKmn1XlyH1lcr1n6g4ypqAMWFyeZG67xhNGTeeQWrNQfOd4Ccxk2PwffaI3ZVvgFcZXShfji0FwtfD6kS+2xAIa4hrtca/9bNkEtEoFaDuOkxOK5u0mrD2dsATIvYXGmdm5oh/ebLANlszRoAcdNj0GPxPfeEGQ3gSmPCDkV8ZFBVLpdqjT+6G3LRDqj1oMegx+K4ekIrNoAr6QRwgjFxQu9iUSteW0suGU4nuRTX2rV1KqRf+011P/Yr8ePB9Fj0mBwL12s1GoAsCZtSlvjPfZALH4F6DXoseky+9643bADrhwY7r8x3Eju09+kswo6yWB+uyjjdo7VrexukX/1FfZ79zacAfEx67NrGwr1xdYXW9dQA1p0Q7FhP37C50RL/uR+yXR9DvQc9Jj02x8DFhrWvdgNYN1RNLkW0hnfMgvQrz9f32d98CsDHpj1wLFyqVWsAa08Idq7TvqnKOovQSS7V99cordqzf+dhaNSgx9ZOARxXd2pdazQANP3C2grI1pbBdQ5yrmvw/hTSGt55BvT88dnGPPubTgG0B9oLx9WFWtcWTgAnahN5tHyQEteK11phiQdXk8slWhOPNvbZv88p4NH9HFd3ag0GOo0TQOda44IFivjIoKpcbtAa3jUben73k8Y++5tPAbgX2hPH1W1a6QSwBk8ANLHGuGCBIj4yqCqXG7TGv7WvIe/8230iQHviuLpOK54A8B9tYo1xwQJFfGRQVS7VtYZ3nQ09L/3I2V/44cAvDKE90d44rm7Smj8BaBP5TZRGER8ZVJVLda2Jb+6p60/9Sf10IO6N4+omrXQCWI0ngCtwAs0ORXxkUFUulbWGd54DPS8+U5v/7+/A7wugvdEeOa4u0bqaTgD4j9uD0eGThhL/+g7IRTtB1UF7oz1yXF2idXX+BLA6P1EaO1cbZuMjhU5yGftzkktFreGdc/AZ9oeg+uj++Xega9NMjqsbtGoNYBWeAFYPMxbZoYiPDKrKpaDWNSdD7P7rIHvkPeUbAO2R9kp75rgqrnWV1gBODHas0iessJOwjI8o5nmc5nJ0fzXQWg1nePOp0P38f4FbBu2V9sxxleBshFas/d4TgDZhh2V8VhdhOX8ZrtX15FJM65rhELvvGsi2v++aBkB7pT3T3msW11V1iOtqRXK4ZlrzJ4CV+oVi7Fw5rBfNX5fwLYfF66vh6se5yuH9KaS1ayM++//MPc/+hVMA7pn2znFVWSudAFaeGCy5uJytkkQnOVXlcppzzUiI3ft5fEb90HUNgPZMeycNnoiFJ/POaAAdxsWOlRJERWus0Eku1ffntNbwTTNd+exvPgWQBo6rqvujBrACTwAVELHV2K7AZ/8vr4dsx2HXNgDaO2kgLRxTBW0FNYDl2ABW0DdsKln4+jbofv674PZBGkgLx1RBW641gGHYAE4yJu1QxEcGVeVSQCs9+//jWsh1fuL6BkAaSIt2CvB7XFXTirUf6Fx2UrBzOX3Dpop1bZgO3S88AV4ZpIU0cWwVM6x9vQEsG+YA2bC+6CRXNZxOctVD66qREL1zNeQiIc80ANJCmkibq2Lh9bwrNAC9G4AtivjIoKpcDdbaddVk6A4+CV4bpIm0+TWuimrNnwBMm2BrnK0YAdE7VkAu1uW5BkCaSBtp5FgrY9gAlp4U7Fp2MiBCMWpOS00LTHNWa4S4llbH1Y9zqcP7a5DWrjXjoOfXB8Grg7SRRr/FVWGtegMobICtcYYBid62FHLJmGcbAGkjjZ35hGVrtBkngKVGV7BAs5XzleGqltNJroZrXTEKel78KXh9kEbS6pu4qp3D2ACWnBzsvBydL8cJWxTxkUFVuRqjNbr/MqyObvB+B+jWtPolrkrnMNa+1gC6jA1Yodm6HEKVueqyvyvGQ2TXQojfcz0kvvVFSP/1JfDLIK2kmbTTPaB74Zm4uimH8w2gc4nRFSzR7loFuORk57gud5irUq1Lh+u2bKRuy/GYu2I0hG86C2J3roPE12+F1I++CT0vPwu5rnasgh6ATFr/4x5kKv6izxr+AtGCbroHeC/ontC9oXtE94ruGd07uofavczf1/x95ryrXqvWAC7DE8ASfQNWaLYuh1BlLkvOpZSEo6Br5VjoWj0BwmsmQte6KRC59lSI3XU1JL66B1I/fER7nZt573XIdaeAR5W9Au8h3Uu6p3Rv6R7TvaZ7TvdeiwHGgmKixYZixDksxnmZ0QDQ9AV2KOIjg6pyXU7P3qfoSbV2MnRdOR3CV8+A8IbTIPqFFRC75zpIff8h6PnVM5B5/Q+QS8S4ShvVHPDeUwwoFhQTig3FiGJFMaPYaTHEWFJMtdj6IYcvE/bJnwCGaxMFvKw/lprrs8YKLdZVxLVEgsuOE61r+RhMjGYIr8cCv+ZUiNwwGyKbzoXo/qUQv28TJB+7F7p/9hikX/klZA//TT+y83DJmww9WswodhRDiiXFlGJLMaZYU8wp9pQDlAtaTrgph52pV+MEcKnhYIMiPjJYF65l+Ey+Cov8qlZ8RsAi33w+RLbOhejeJRC/+wZIfvMOSP342/ja82d4zHwDcvEoF4/XTw0YY4o1xZxiTzlAuUA5QblBOUK5QjlDuUM51NAcrhCFfLQGcCmeAKwcL+1F89cVbaxovVNcXUtG4Ou/8RiwNjz6naEX+M7FELt1OcTvuRGS37gNUs/g6/Kffw8yh/4vZD9+l6uAR8lBuUE5QrlCOUO5QzlEuUQ5RblFOUa5Rjmn5Z5DdVGqRqqqCzEuvQH0f/Dh/bDUXL8HscW+6+S48PqysXhka4HIhjMhsuUCiO6+BGK3rda6d+Lh/ZB66t+h54UnIfP/XoLsh4f4DTgejr4RSTlFuUU5RrlGOUe5RzlIuUg5SblJOUq5KpTTFdeDM/WqN4BL8ARgu8Bug5Wj1bXw2mkQuY6KfC7E9i+D2O1rIP6vW/GG3wqpJx/C13RPQPr3L2CnftUTvzCDh8ubA+Yg5SLlJOUm5SjlKuUs5S7lMOUy5TTltnzBVoZCvpdQA7gYTwCX4MQlNGGNIj4ymP86csMcvFHrIP4vWOQP7YXU/zyoF/mLz2o3NvvRO5xlPNz5kgJzV2sOmMtac8DcphynXKecp9wvrgenUMjnYqMByBSs0xuM3rIcevAG5aJdav3Nex48anJkyGq5TjlPuV/rJ1hbLDSAi3GiQRZeNRm74i36M72ffhqOh08bQE7Ldcp5yv1G1p7eAC4aHkTDiRHGBSu098lziHGN6PN1eOUkSDy4V3+HnpsADy8XP+Y45TrlvFU9OFNj5bmo9rEB0AkAL+RJrVDERwaL5sIrsAk8sBuyRz7gJsDDm8WPuU05Trlerh6qRgEfqv3eE8BFRme5KL+hYix9rbMMdlliibnlEyFx/07Ihj6mO8ZJw8Mr1a/lNOU25bhwPVRVY+W59BPA4uFBzWGxsQkrFPGRQYtr4WXNkLhvG+TCHZw3PLxR/pjLlNOU27L1UDEK+FDtaw2g09hAMdpZpyTKcHUtnQDxuzfxj+bycH/xYw5TLlNOq1RjnYtNDaBLgKDeFl4yHmJ7V0D2o79xFvFw5aDcpRymXFaxxvQGsBAbwCKcUNEuHg3RHUu4CfBwZfFT7lIOq1pfVPtGAxhpTFqh2UZWiZJcF5/CTYCHS4v/lNrVhQNcRgMYGexaaFywQlpQzkcWZXwvoiZwOTcBHi4p/su1nK1ZPQhheR+q/UDnhdQAaEJxW4xN4OYlkHnvTf45AR4KvtuX03KTcpRy1Q01RbWvN4ALR+iTlmiYrY8kVrJm8WiI7VoOmUN/xVbL/2+AhypP+1ktJyk3KUfrVg9V1mtvA1hoXjCyL4mVLZREp7iwu8b2robMm38ByGQ4+Xg0dmAOUi5SThae+RtRFxVw9TaABcakHYr4yGA1axeNgdj+dZB5/RX910rz4NGQ4k9rOUi5SDnZsHqosF71BrBgZDB84ShtohjtzGpNPbgIw3jD41+4CtJ/eQlyyQQnI4/6vuTHnEv/+bcQv/VKLRcryeFG1xjVvtYAyi1W1hbjSeDmpdDzyx9jQOKclTzqVPxxLeeimy8G19YOnQC0BjAfTwALjK6wIN9JSmHpa+Ey2GWJ8lxWnNEtl+pNgH9HP49aFz/mmFb8mHNO5rBdrVRWY+W5qPYDnfNGBbvmo8N8vFBAGRtZBkc5yGlt0S2XQU+QmwCPGhd/8MdartUih+tTY71rqfa1BhCueGO9Fi5CJ7lEOXubAL8c4OF08cclir/yHK5njRUaQNc8nPSIRTdfqjcBfk+Ah5Ov+an4Mbe8VCtaA+iaqzeAsDFphSI+MlhLrtimS/B12k8gl+JPB3hUWfyYQ5RLlFP1zOFqUchnLp0A8J/wvNH0jX7BAu2u6Ti6CO395bhGS3NFb7oYen71U/4DITwqL37MHcohyqVG5HA1NSbCRbWvnwBoYq5BaoEiPjJYD67oDYswgP/Lf9iTh/zAnKHcoRxqZA5XioI+eAK4AE8ANHGBccECRXxksF5c0Q0LoPsn/80/MchDovjT0P3Ut7XcUSGHK0ERH6r9AH4T1CYuMC5YoIiPDNaTK7LmbEg99hAnNg+hkXrsQYisOlOpHJZFQR/jBKBN5DdRGkV8ZLDeXJGlMyD1xMOc3Tzsix9zhHJFxRyWQREf/QRwPp4APocTaHYo4iODjeCKLD0VUo9/jbOcR+nix9ygHFE5h0VRyOd8owGETZvwtp2iBbj7MW4CPPoOygnKDcoRv9RDoQF0nZ+fKI19FzmDDeMymkCKmwCPwmv+/DP/Ke7IYYfqVW8A5+EJ4HwUThcs0e5apdhILmwClxsnAf71Yv4dGHvtmR9zIZ8X7snh6jmp9rEBjA6iGROlMYxYzkcWVeCKLMGTwHce4p8T8OVHfT1a7CkH3JzDlijgQ7XfewIwFXsBzzNj0dz5FWKBRw2uyNLToPu7X9X/AxGfBnzxrE+xpphT7L2Qw5XWa+8J4Fzd0QrD5xpm4yOKeR6nuarZX2TJLEh+/R7Ith/mJuDx4qcYU6wp5o3Ouz6c5zlcYwJc+gng3NHBPqLMWMrOqxKd5HSQK7J4OiTvPwDZjz/kJuDV4sfYUowp1l7MYVlOqn29AZxrs9BHFlncAsl/uwOyH73PTcBrxY8xpdhSjDnXddMbwBxuAP2awANfguyH73IT8ErxYywpplz8RQ1gDjWAc7ABzMEJtoJFFrViwtzJTcAzxX+nFlPO7b5GtY8N4BRsAGOMSTsU8ZFBVbl0rdwEvFb8/svhcj5U+9gAxgTD5+DEOWOqNIPcEa4xvVxznOYS35/eBOjlwHvcBFxX/O/px34q/kpyeI5iOex4jdEJYAw2gLOpATi1Qe9ZZCE2gX/7ImQP8xuDrin+w+9rMaPYcQ5bG9V+bwM427hghSI+MqgqVwmtWhP41zsg+8E7/EdJVR4YG4oRxUorfs5hW9QbwFnYAM42bYKtpEUubIHEbZsh8xb/ZWJVi59iQzGiWHHOljeqfa0BRM4eC+GzMMkN1ByKsdScaY0VWq2rNVdVnFZc5zZDfOc1kDn0Br8cUOzYTzGh2FCMGpnDFXPWK4dNawsNoLABtvI2ZwLEt18J2XcPceGp8uSPsaCYUGw4R8Wt9wRwltEVitDOrNY0mqsu+6MmsGWt/ukAj8YWP8aAYhExit/Teecwl94AzhwbDM9Gp9l4wRZFfGRQVS5BredgE9i6niuwwYNiQLHgHJbXSrWvNYCIsQErNFukSgw7yBmu8f7K+UYvnMEV2OBBMXA6rn7J4UIDCJ9pdAUrNFs533pyNXh/8S3ruAIbfQLAGKicIyrnsN4AzsATwJk0gV3BAs0WcQhV5hLinD0Okl++lSuwwYNiQLHgHJbXSrUfiOA/4TOMCzYo4iODqnIJa8WkS32bf7FoowfFgGLBOVyBVmoAXafTCSB/Ay3wjHGa2fqIosHjCNeZJi4H9yfENXs8ZP78B67ABo/Mn3+vxYJzWF4r1X4ggv+ETzce3AZFfGRQVS5hrWdNhFw8yhXY6I8BP/lYiwXncAVaT8+fAMo6jtPMkQ2ebtrg6QpxSWqNXnQmV58qnwRgLDiH5bX2PQGc3ndDOo4roPnr0r6iOM60QfW4RLUmtl/DlafIoFhwDleiFRtA+DQ8AZRcMM5mg9WjqlyiWlMPfJkrT5U3AjEWnMMVaD2NTgCz8ARwGk6cRhM6ylhYEN3MVYqz5wePc+UpMigWnMMVcM4yGkCkAiK/W+bVP3HlqfJJAMaCc7ICyzeA8Kz8RIVGHaUIK+aa1Z8r7DRXlVojsydDrquTK0+RQbGgmDidw2FFcjhSgxzW90MN4NRxwcis8cZFOxTxkUFVucprjV32OYDuFFeeKgNjQTHhHJbUirXf2wBOLUyWRhEfGVSVS0Br4pZt+MKT/6CoOm8C9EBi542cw/JasQHMHBcMaxvIb8QK7X3CRRgpi+OFOZ3kckJr6pGH8IVnhgtPmTcB0pD62v2cw7JaZxoNQLsw09iEFYr4yKCqXAJae370FP9eQKV+HDCrxYRzWFZrvgHMNG2Craxl3+bfCajWu4A5LSacm7LGDUDaohfMglxnBxedaj0AY0Kx4RyVbQAzsAHMwG/YhCy++lLIhcNccao1AIwJxYZzVMaoAbRRA5hgTBSjnU2oEN3E1Z8zecctkIvHueJUawAYE4oN57AEZ5vWACYEI235CQs0k7RViYVNOMzl6P6sfbof+SpAin8GQLmBMaHY1D2HHeOsXw4XEGs/EGmlBkATbCLW8/yzAOk0F5xqA2NCseEclbDWfANoHW9MWGCfReOdQZW5bDizf3uHi03VTwMxNpzDElq1BtAyIRjVuwFYofZ1q72PLCrNZcEZW3Ih5D4+zJWm6vsAGBuKUcn4trog7xziEtbaYjQANH2BHYr4yKCqXDZa49dfBbn2dq40VRsAxoZixDks7GOcAFqbtQkrNFs533py1Xt/qbvuhFyEPwJUtgFgbChGnMPCXKYTAFtZ63n8vwCSSa40VQfGhmLEuSpsxgmgxegKRRjpg33nopIY6YeVc0VrymWtNf1/fs6fACj+SQDFqFY5HK1TDkdqmMNFiA1gWnMwMh0dp+NEP7SzCZLY7CBnc533hzfszBmQffMNLjLVPwl443WIntbKOSzChbWvNYCo4WSHIj4yqCqXldbE2pWQfZ//HLjyDeC9dyG+YgnnsIgvNYAonQCmTTAmEKdpF/phqTkdTWuL0Wad5bXpdpxOcslpTW7fzB8BuuSjQIqVYzk8vcE5PM25HO7HpZ0ApuIJwHaByAblUVUuK62pf7kXcl1dXGGqNwCMEcWKc1jAdyqdAPAf/CI/YYkiPjKoKpeV1p4fPMn/B8ANA2NEseIcFvDVGsAUowFMnQh2KOIjg6pyWWlN/+qX/FuAXPEmQFaLFeewgO8UegmA/2gTU4wLFijiI4OqcpXSGjt/DmRffdVzPzWX/tEzmnntpxspVhQzzuGyvsYJgCamGBcsUMRHBlXlKqU1sf4KyB562xuF39mJRf9DSO7bDbHFCzSjr2mOrnmiAWCsKGacw+V8jQaQ3wBbaUvu2QXZjz50d+HH45D+2XOQ3LUD4osWQLR1Wq9G/Jrm6Br5uP0XnmQ/eB+S27dy7pY1agCT8QQwGb9BK0Y7i0hi1EHOaJ331/3gV9z7CUBPD2RefglSB26D2KKFEG2ZZq0Vr5EP+dIat/7tA/pLQd0P3M85XJbLaABRiU350dLPHHRfMWSzkPnjHyB12xcgvngRRE+dIa4ZfWkNrSUO1735ibGimHHulrP8CWCS0RWKsGQnmVQdOslZ6/1pc20tkKFPAFz0a8CzR45A6tb9EJs/D6Iz28S1FiOuJY7UP/2DxumeI0BOixnFTlirR3PYnpMawEQ8AUyaBFGasESzTaoSneSq/f7ieCTO/MkdfwmY3s3vfvBBiC+g1/gtjt232Iw2jZO43fKJAcWMYsc5bOM7UWsAE4ORifqEFZrJyvmKcU1yhKuX00muvlqTGzZA9q231C78UAh6Hn4YYvPwGb+NnvEnVaTVFskfuekx6LHoMZU+BWHMKHa1yOHa5N0kh2tMwBdrPxBtNk4A+cVWKOIjg6pyFWlN7tsH2Q8+ULPwEwlIHzwIiWXLINrSUrVWYcTHosekx6Y9qPlJwAda7DiHbbCZTgDNeAJopg1gV7DAaLP1tUpRWa4ird0PPqTWHwLJZrVPJNJPPw3xpVj4U6dWpVWzSu8XPjbtgfaifUqi0JuFFDOKXZ/9Nk/yRQ4La22eqDeAQjJYYXPfZKkKneSaWCOuvE2aoiW3En8JGPeQi0Qg/cILkFh/JRbfdHVigXuhPdHeaI+q3C+KXXTiZPflXd248g2gucRCNojNPhsywWBjEzmd1l5vZ371a0hu2w6xGaeqe79wb7RH2qv2HkGDf3tS+ucvQGzmLM5lS+MGYGvxS5dA5ve/b9xRv6MDMr/Gwt+5C2Knnuaexol7pT3T3klDo14aUOwohpzLdg1gAjaACfgNWz9LbtkG2bffrn/hh7DwX34ZUnfdDbGz5rj2/tHeSQNpIU31bgQUO4oh57KVaQ1gEjaAycaEHYr4yKCqXL1aUwe+CLmP6vdbgOiNq8xvX4Tue/8Z4nMX1FVrLWNBWkgTaavnG6oUO4qhn3O4DGIDGD85GB1vTBCOnyxu+TW2KGOTynPWcX/dX30YctFY7RM1mYTsH/8E3Q88BPF5CxsfixrFlbSRRtKaq8OvV6fYUQz9nMO2+8PaD0THUQOQJfGBTWmF9FMHa174mT/8Ebof+Q9IrFzjm3tLWkkzaa91I6AYUiw5p0vYuHwDGDcpP2GNdtc0nFSEZfzHy3A6ySWmNX7WeZB+7vmavbOf/cur0P3wNyB5xTqITpzWUK0NiStqJu3d//4IZF97rWafGFAMKZZ+zOGyXFoDGEsNwLSITbPE0pWQefl3zhf+a69Dz6PfgeTV10Ns6gzf3+fYtJmQvGGTdk/o3jjdCCiGFEvO6RI21twAxhYmS6OID2Js3BQhP2W5DK3JrTsh+/Yhhwo/A9nX34Ce/3gUktdvhNhpZ0Ns/FRltDY6FnQv6J7QvaF7RPeK7pkjnwS8+RYkN27zZQ4L+BoNYKxpE2yapb70T5A70u7IO9E9T/wPPstthtjpc/D4NZXvr5VRI8B7RPeK7pkTn8DkPv4EUrffyfe2tOkNIDZW7yx5lLHitdVwleN0kqscZ8/XvgHQ3V154n1yBNLf+z4kt+yA+HnzIdY8XVmtqsWV7hXdM7p3dA/pXlY8MIYUSz/msAAXNoAxU4LRMXhxDE7YooiPDKrKhTen7UxIP/lUZYUfi0H6p89B8satED93PkSbW5TWqnRc8d7RPaR7SfeU7m1Fr8AwlhRTP+WwoK/eAGLGBorRzmKSWC8uJzjjn1sEGdlPAOgjveCvIbVrv7Y+Vih8tbW6YX90L+me0r2leyz7J9oplrTez7GwWGOcAE6ZrE8UYxF5nzmrNeXQWK8qF2Fi5XrI/E7w/wD0pCHzmxchdfNePLZeCLHJM12l1VVxxXtL95juNd1zuvdCDQBjSTH1Uw6LaNUbwGg8AZxCE+hogyI+MqgqF2Fyy07IHnqn/HH/vfchte8AxM+aC7FJM1yp1Y1xpXtN95zuPcWg7CcBGEuKqZ9yWMh3tNEA0PITpbHvourQvMHRzmDM4f1133k35MIR64T6+BPoues+iJ1xvpaMbtbq6rhS08UYUCwoJta/KzEE3bfd6ascFuHSG8AoOgFMNS5YoXnxVEdQVS7S2f2Vh0v+FuBcPAHd930F4qdj4TfP8IRWL8SVYkExodhQjEr9luDuBx72VQ4LaR1lNAA0iI3WJkpjfoN2PpKoKld89lxI/+CZPslD/6mk57vfg/jciyE2vtUzWj0XV4wNxYhipf0nLlMTp5hSbP2Qw6JxLTSAsqSjpmrm2Ab1B1eSK7FwKWSe/4X+m3jw6EjJlJh/GUTHTPecVs/GFWNFMdMaAf0egkxGiynF1g85LBFX0wlgVO/iYjRbOd96ctWCM7FsnZYs6R/+LySWXAGxsS2e1er5uGLsEsvXQ+a5F/QGgLH1Qw6LcvWeAIoW+dkS85dA8vK1EJ80i++HR4xiSTGl2PL96NMcsAGMxBPASJxAK0Y7i1aIbuJirazVy1qp9rUGEBMgYGNj85bpDWAEngBGGBMGxkaUt+I1tihiI8U5ndxfzOn9sVbWWmetle6Paj8QHY4ngBHTDCc7FPGRQVW5WCtr9YdWqn1sAFODaPoFOyzjEy3Ccv4yXNF6clWgNcpaWWsNuWI10kq1H4iePDUYG55fZIciPjKoKhdrZa3+0Eq139sATjYWWaGIjwyqysVaWatPtPY2gJNNm2BjY/OFcQNgY/N9AzgJG8BJOMHGxuYro9o3GsB0Y3K6BMF0SXSSU1UuL2jlWPhFq9EApgVjw4wLxdiP3DQ3TBL7bbgKrmE2XI7sj7WyVu9rpdoPRE+cFowPo4npUIx2Fq8Q3cTFWlmrl7VS7WsNIHbiNN3RCs1WzleGq1pOJ7lYK2v1mVa9AZxgnABONLpDCewls/YRxTyPE1xxE5ej+2OtrNUHWqn2tQYQO8FYaIVmO6FKdJJL9f2xVtaq8P4KDSB+Yos2YYV21ypFZblO9JfWfDJ4XauGftIqwNn3BMDGxuYr6z0BnGB0BRsU8ZFBVblYK2v1i1atAcSGtgRjQ3FiKF6wRN3sfURR54k7wukkF2tlrf7SSrUfiOI/fR+8NIr4yKCqXKyVtfpFK9U+nQCejg0xuoIZhxC2FND8tY4l1thi3/W142qR5Kq31hbWylr77a8RWqn2A7HBLbfHSy4oI7ZKVJWLtbJW32jF2g8kBrVcgV/kJyxRxEcGVeVirazVL1qp9gOJgS3D44NbQ9qFwa1ghXbXKkFVuVgra/WJ1hDVfiA2qOVotIPahUGGQwm0u1YJqsrFWlmrT7QepNqnlwBHoa3Ab1LaxUGGUxHaXasEVeVirazV61qp1qnmqfYD0NQSSA5sGRob1PpcfiNsbGwets+2PpMa2DKEal8b8UEtn0ZbhBcOo4GMxcpgJeY2Li9q5Vh4Vuvh+MCWefj6/1MB84gPbD0WbS+SJuMOJA4bG5taptU21jjVeqB4JJta/i4+oOXk2MDWe9DKkw2UQ6ENNoCLtbJWv2il2qYap1oPlBqxppa/jw1oGRUboDUB6hZgbW2S6CRXLThZK2v1plaqZa2mqbaxxgN2gxziTS0nxwfgUWEAvV5o67MhJKoKezfcVjVnbbi8r7Uvp/+0qpl3NdJKNUy1jDVdtvj7vBxoaj0WbR4ufgaJUwYZOIaqcuWDwVpZq4u1ajWLtavVMNay5bHfbiSaWj6VamodkmhqW4EkB+MD2kKI+CD4QJXigDyavlaKq7V6LtW1Nvlc6wBPa6UaPUg1q9du0bv9soM+K0Syo5D06ERT63D8+op4U9vtsabWp9GCbGxsDbenqSapNqlG9VptO6rwOb/N+P/Rhc2Kn0c0aAAAACV0RVh0ZGF0ZTpjcmVhdGUAMjAyMS0wNC0yOFQyMToxNDowNyswMDowMGTJXEQAAAAldEVYdGRhdGU6bW9kaWZ5ADIwMjEtMDQtMjhUMjE6MTQ6MDcrMDA6MDAVlOT4AAAAAElFTkSuQmCC\",\n  \"name\": \"file-type-favicon-icon-256x256-6l0w7xol.png\"\n}",
                        Encoding.UTF8, "application/json"));

            var response = await Http.Send(httpClient, request);

            return response;
        });

        NBomberRunner
            .RegisterScenarios(scenario)
            .Run();
    }

    public static void CreateEventTest()
    {
        using var httpClient = new HttpClient();

        var scenario = Scenario.Create("http_createEvent", async context =>
        {
            var request =
                Http.CreateRequest("POST", "https://localhost:7172/api/events")
                    .WithHeader("Accept", "application/json")
                    .WithHeader("Authorization",
                        UserToken)
                    .WithBody(new StringContent(
                        "{\n  \"eventDto\": {\n    \"name\": \"Test meeting\",\n    \"pictureUrl\": \"\",\n    \"description\": \"test test test test test test test test \",\n    \"startTimeUtc\": \"2024-07-01T17:15\",\n    \"minRequiredAge\": 18,\n    \"ticketPrice\": 5,\n    \"totalTicketCount\": \"100\",\n    \"location\": \"Ternopil Lake, Тернопіль, Ternopil Oblast 46001, Ukraine\",\n    \"locationLongitude\": 25.579174,\n    \"locationLatitude\": 49.551005,\n    \"customEventType\": 1,\n    \"customEventLocationType\": 2,\n    \"customEventCategories\": [\n      4\n    ]\n  }\n}",
                        Encoding.UTF8, "application/json"));
            var response = await Http.Send(httpClient, request);

            return response;
        });

        NBomberRunner
            .RegisterScenarios(scenario)
            .Run();
    }
}
