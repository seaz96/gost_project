using FluentAssertions;
using GostStorage.Services.Services.Concrete;

namespace GostStorage.Tests
{
    public class TesSha256PasswordHasherTeststs
    {
        private Sha256PasswordHasher _hasher;

        [SetUp]
        public void Setup()
        {
            _hasher = new Sha256PasswordHasher();
        }

        [TestCase("helloworld", new[] { "heloworld", "hhelloworld", "abc123", "hello world" })]
        [TestCase("abobus123", new[] { "abobus 123", "abobus122", "abobus", "ab0bus123" })]
        [TestCase("s1mple", new[] { "s!imple", "simpl3", "asddsaa", "simple" })]
        [TestCase("qwuipss443", new[] { "qwuipss", "qwuipss 443", "qwp443", "443" })]
        public void Should_not_verify_with_wrong_password(string initialPassword, params string[] passwords)
        {
            var hash = _hasher.Hash(initialPassword);

            foreach (var password in passwords)
            {
                _hasher.Verify(password, hash).Should().Be(false);
            }
        }

        [TestCase("helloworld")]
        [TestCase("abobus123")]
        [TestCase("s1mple")]
        [TestCase("qwuipss443")]
        public void Should_verify_with_correct_password(string password)
        {
            var hash = _hasher.Hash(password);

            _hasher.Verify(password, hash).Should().BeTrue();
        }

        public void Should_work_with_special_symbols()
        {
            var password = "!@#$%^&*()_+{}:\"<>?,.;'[]-=";

            var hash = _hasher.Hash(password);

            _hasher.Verify(password, hash).Should().BeTrue();
        }

        [TestCase("hello world")]
        [TestCase("1234567")]
        [TestCase("asdsaertgjnmkfsddsffeirtujgkerjgifrsfd")]
        [TestCase("                   sssssssd            dsadasd")]
        public void Should_has_const_hash_length(string password)
        {
            var hash = _hasher.Hash(password);

            hash.Length.Should().Be(44);
        }
    }
}