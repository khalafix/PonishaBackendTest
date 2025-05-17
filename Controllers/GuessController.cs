using Microsoft.AspNetCore.Mvc;
using Ponisha.Models;

namespace Ponisha.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuessController : ControllerBase
    {
        [HttpPost]
        public IActionResult CheckGuesses([FromBody] GuessRequest request)
        {
            var results = new List<GuessResult>();

            foreach (var guess in request.Guesses)
            {
                int exact = 0;
                int misplaced = 0;

                var secret = request.Secret;
                var guessChars = guess.ToCharArray();
                var secretChars = secret.ToCharArray();
                var usedInSecret = new bool[secret.Length];
                var usedInGuess = new bool[guess.Length];

                // مرحله ۱: بررسی exact
                for (int i = 0; i < secret.Length; i++)
                {
                    if (guessChars[i] == secretChars[i])
                    {
                        exact++;
                        usedInSecret[i] = true;
                        usedInGuess[i] = true;
                    }
                }

                // مرحله ۲: بررسی misplaced
                for (int i = 0; i < guess.Length; i++)
                {
                    if (usedInGuess[i]) continue;

                    for (int j = 0; j < secret.Length; j++)
                    {
                        if (!usedInSecret[j] && guessChars[i] == secretChars[j])
                        {
                            misplaced++;
                            usedInSecret[j] = true;
                            break;
                        }
                    }
                }

                results.Add(new GuessResult
                {
                    Guess = guess,
                    Exact = exact,
                    Misplaced = misplaced
                });
            }

            return Ok(results);
        }
    }
}
