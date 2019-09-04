using UnityEngine;
using System.Collections;

namespace CardGame.Models
{
    public class Validator
    {
        public bool IsValid { get; private set; } = true;

        public void Invalidate() { IsValid = false; }
    }

    public static class ValidatorExtensions
    {
        public static bool Validate(this object target)
        {
            var validator = new Validator();

            //

            return validator.IsValid;
        }
    }
}
