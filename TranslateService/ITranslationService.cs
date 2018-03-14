using System.Threading.Tasks;

namespace TranslateService
{
    public interface ITranslationService
    {
        Task<string> RussianToEnglish(string text);
        Task<string> EnglishToRussian(string text);
    }
}