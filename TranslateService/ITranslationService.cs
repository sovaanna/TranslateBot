using System.Threading.Tasks;
using TranslateService.Enums;

namespace TranslateService
{
    public interface ITranslationService
    {
        Task<string> TranslateRussian(string text);

        Task<string> TranslateEnglish(string text);

        Task<string> Translate(string text, Language source, Language destination);
    }
}