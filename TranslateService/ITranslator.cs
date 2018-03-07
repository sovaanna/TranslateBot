using System.Threading.Tasks;

namespace TranslateService
{
    public interface ITranslator
    {
        Task<string> RussianToEnglish(string text);
        Task<string> EnglishToRussian(string text);
    }
}