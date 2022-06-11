using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WordListApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>


    public class Word
    {
        String wordName;
        String? parts_of_speech;
        String? en_interpretation;
        String? cn_interpretation;
        String? sample;
        String? key_knowledge;
        bool isnew;

        public Word(string wordName, bool isnew = false)
        {
            this.wordName = wordName;
            this.Isnew = isnew;
        }

        public string WordName { get => wordName; set => wordName = value; }
        public string? Parts_of_speech { get => parts_of_speech; set => parts_of_speech = value; }
        public string? En_interpretation { get => en_interpretation; set => en_interpretation = value; }
        public string? Cn_interpretation { get => cn_interpretation; set => cn_interpretation = value; }
        public string? Sample { get => sample; set => sample = value; }
        public string? Key_knowledge { get => key_knowledge; set => key_knowledge = value; }
        public bool Isnew { get => isnew; set => isnew = value; }
    }

    public class Phrase
    {
        String wordName;
        String? parts_of_speech;
        String? phrase_usage;
        String? en_interpretation;
        String? cn_interpretation;
        String? sample;
        bool isnew;

        public Phrase(string wordName, bool isnew = false)
        {
            this.wordName = wordName;
            this.isnew = isnew;
        }

        public string WordName { get => wordName; set => wordName = value; }
        public string? Parts_of_speech { get => parts_of_speech; set => parts_of_speech = value; }
        public string? Phrase_usage { get => phrase_usage; set => phrase_usage = value; }
        public string? En_interpretation { get => en_interpretation; set => en_interpretation = value; }
        public string? Cn_interpretation { get => cn_interpretation; set => cn_interpretation = value; }
        public string? Sample { get => sample; set => sample = value; }
        public bool Isnew { get => isnew; set => isnew = value; }
    }

    public partial class MainWindow : Window
    {
        /*需要做两件事，一件是从文件读取一行数据，检测分割并加入链表，一个是将链表中为新的数据换成一行，写入文件末尾*/
        StreamReader? reader_words = null;
        StreamWriter? writer_words = null;
        FileStream? fs = null;
        String words_txt = "words.txt";
        String? tmp = null;
        Word? tmpw = null;
        Phrase? tmpp = null;
        String match_word = @"[^[\];()]*[:[][^[\];()]*[;][^[\];()]*[;][^[\];()]*[;][^[\];()]*[;][^[\];()]*[]]";
        String match_phrase = @"[^[\];()]*[:(][^[\];()]*[;][^[\];()]*[;][^[\];()]*[;][^[\];()]*[;][^[\];()]*[)]";

        List<Word> wordlist = new List<Word>();
        List<Phrase> phraselist = new List<Phrase>();
        List<BorderExWord> borderexwordlist = new List<BorderExWord>();
        List<BoderExPhrase> borderexphrlist = new List<BoderExPhrase>();
        HashSet<string> wordSet = new HashSet<String>();
        public void WordsLoad()
        {
            initStream();
            //read a line from txt;splite to a word or phrase
            tmp = reader_words.ReadLine();
            while ((tmp = reader_words.ReadLine()) != null)
            {
                if (Regex.IsMatch(tmp, match_word))
                {//word
                    tmpw = StringtoWord(tmp);
                    wordlist.Add(tmpw);
                    addWordItem(tmpw);
                    wordSet.Add(tmpw.WordName);
                }
                else if (Regex.IsMatch(tmp, match_phrase))
                {//phrase
                    tmpp = StringtoPhrase(tmp);
                    phraselist.Add(tmpp);
                    addPhraseItem(tmpp);
                }
            }
        }


        public void addWordItem(Word word)
        {
            BorderExWord borderExWord = new BorderExWord();
            borderExWord.Wname = word.WordName;
            borderExWord.ExWordName.Text = word.WordName;
            borderExWord.ExWordType.ToolTip = word.Parts_of_speech;
            borderExWord.ExWordEN.ToolTip = word.En_interpretation;
            borderExWord.ExWordCN.ToolTip = word.Cn_interpretation;
            borderExWord.ExWordEg.ToolTip = word.Sample;
            borderExWord.ExWordKey.ToolTip = word.Key_knowledge;
            borderexwordlist.Add(borderExWord);
        }

        public void addPhraseItem(Phrase phrase)
        {
            BoderExPhrase boderExPhrase = new BoderExPhrase();
            boderExPhrase.Pname = phrase.WordName;
            boderExPhrase.ExPhraseName.Text = phrase.Phrase_usage;
            boderExPhrase.ExWordType.ToolTip = phrase.Parts_of_speech;
            boderExPhrase.ExPhraseEN.ToolTip = phrase.En_interpretation;
            boderExPhrase.ExPhraseCN.ToolTip = phrase.Cn_interpretation;
            boderExPhrase.ExPhraseEg.ToolTip = phrase.Sample;
            borderexphrlist.Add(boderExPhrase);
        }

        public Word StringtoWord(string str, bool isnew = false)
        {
            int index = str.IndexOf(":[");
            Word word = new Word(str.Substring(0, index), isnew);
            int index1 = str.IndexOf(";");
            word.Parts_of_speech = str.Substring(index + 2, index1 - index - 2);
            index = index1 + 1;
            index1 = str.IndexOf(";", index);
            word.En_interpretation = str.Substring(index, index1 - index);
            index = index1 + 1;
            index1 = str.IndexOf(";", index);
            word.Cn_interpretation = str.Substring(index, index1 - index);
            index = index1 + 1;
            index1 = str.IndexOf(";", index);
            word.Sample = str.Substring(index, index1 - index);
            index = index1 + 1;
            index1 = str.IndexOf("]", index);
            word.Key_knowledge = str.Substring(index, index1 - index);
            return word;
        }

        public string WordtoString(Word word)
        {

            return word.WordName + ":[" + word.Parts_of_speech + ";" + word.En_interpretation + ";" + word.Cn_interpretation +
                ";" + word.Sample + ";" + word.Key_knowledge + "]";
        }

        public string PhrasetoString(Phrase phrase)
        {

            return phrase.WordName + ":(" + phrase.Parts_of_speech + ";" + phrase.Phrase_usage + ";" + phrase.En_interpretation +
                ";" + phrase.Cn_interpretation + ";" + phrase.Sample + ")";
        }

        public Phrase StringtoPhrase(string str, bool isnew = false)
        {
            int index = str.IndexOf(":(");
            Phrase phrase = new Phrase(str.Substring(0, index), isnew);
            int index1 = str.IndexOf(";");
            phrase.Parts_of_speech = str.Substring(index + 2, index1 - index - 2);
            index = index1 + 1;
            index1 = str.IndexOf(";", index);
            phrase.Phrase_usage = str.Substring(index, index1 - index);
            index = index1 + 1;
            index1 = str.IndexOf(";", index);
            phrase.En_interpretation = str.Substring(index, index1 - index);
            index = index1 + 1;
            index1 = str.IndexOf(";", index);
            phrase.Cn_interpretation = str.Substring(index, index1 - index);
            index = index1 + 1;
            index1 = str.IndexOf(")", index);
            phrase.Sample = str.Substring(index, index1 - index);
            return phrase;
        }


        public void initStream()
        {//check is there a  file named words.txt,if it doesn't exist,create and init wordslist, or open and init wordslist
            fs = new FileStream(words_txt, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            reader_words = new StreamReader(fs, Encoding.UTF8);
            writer_words = new StreamWriter(fs, Encoding.UTF8);
        }

        public MainWindow()
        {
            InitializeComponent();
            List<String> partsofspeech = new List<String>() {
            "Nouns", "Pronoun", "Adjective", "Adverb", "Verb-vt", "Verb-vt", "Numeral", "Artical", "Preposition", "Conjunction", "Interjection","Phrase"};
            cbxPartsOfSpeech.ItemsSource = partsofspeech;
            cbxPartsOfSpeech.SelectedItem = cbxPartsOfSpeech.Items.GetItemAt(0);
            rbtNone.IsChecked = true;
            WordsLoad();
            wordbox.ItemsSource = borderexwordlist;
            phrasebox.ItemsSource = borderexphrlist;
            count.Text =wordSet.Count().ToString();
        }

        private void rbtWord_Checked(object sender, RoutedEventArgs e)
        {
            label1.Text = "EN Interpretation :";
            label2.Text = "CN Interpretation :";
            label3.Text = "Sample Sentence :";
            label4.Text = "Key Knowledge :";
        }

        private void rbtPhrase_Checked(object sender, RoutedEventArgs e)
        {
            label1.Text = "Phrase :";
            label2.Text = "EN Interpretation :";
            label3.Text = "CN Interpretation :";
            label4.Text = "Sample Sentence :";
        }

        private void rbtNone_Checked(object sender, RoutedEventArgs e)
        {
            label1.Text = "";
            label2.Text = "";
            label3.Text = "";
            label4.Text = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String tmp;
            Word? _tmpw = null;
            Phrase? _tmpp = null;
            if (rbtNone.IsChecked == true)
                return;
            else if (rbtWord.IsChecked == true)
            {
                tmp = wordname.Text.Trim() + ":[" + cbxPartsOfSpeech.Text.Trim() + ";"
                    + txb1.Text.Trim() + ";" + txb2.Text.Trim() + ";"
                    + txb3.Text.Trim() + ";" + txb4.Text.Trim() + "]";
                _tmpw = StringtoWord(tmp,true);
                wordlist.Add(_tmpw);
                addWordItem(_tmpw);
                wordSet.Add(_tmpw.WordName);
            }
            else if (rbtPhrase.IsChecked == true)
            {

                tmp = wordname.Text.Trim() + ":(" + cbxPartsOfSpeech.Text.Trim() + ";"
                    + txb1.Text.Trim() + ";" + txb2.Text.Trim() + ";"
                    + txb3.Text.Trim() + ";" + txb4.Text.Trim() + ")";
                _tmpp = StringtoPhrase(tmp,true);
                phraselist.Add(_tmpp);
                addPhraseItem(_tmpp);
            } 
            rbtNone.IsChecked = true;
            SearchContentChange(null,null);
            count.Text = wordSet.Count().ToString(); 
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            fs.Seek(0, SeekOrigin.End);
            foreach (var word in wordlist)
            {
                if (word.Isnew)
                    writer_words.WriteLine(WordtoString(word));

            }
            foreach (var phrase in phraselist)
            {
                if (phrase.Isnew)
                    writer_words.WriteLine(PhrasetoString(phrase));
            }

            writer_words.Close();
            reader_words.Close();
            fs.Close();
        }

        bool canshowW() { return true; }

        private void SearchContentChange(object sender, TextChangedEventArgs e)
        {
            if (searchBox.Text == "")
            {

                wordbox.ItemsSource = null;
                phrasebox.ItemsSource = null;
                wordbox.Items.Clear();
                phrasebox.Items.Clear();

                wordbox.ItemsSource = borderexwordlist;
                phrasebox.ItemsSource = borderexphrlist;
            }
            else
            {
                wordbox.ItemsSource = null;
                phrasebox.ItemsSource = null; 
                wordbox.Items.Clear();
                phrasebox.Items.Clear();
                string s=searchBox.Text.ToLower();
                foreach (var word in borderexwordlist)
                {
                    if (!wordbox.Items.Contains(word)&&word.Wname.ToLower().Contains(s))
                        wordbox.Items.Add(word);
                }
                foreach (var phrase in borderexphrlist)
                {
                    if (!phrasebox.Items.Contains(phrase)&&phrase.Pname.ToLower().Contains(s))
                        phrasebox.Items.Add(phrase);
                }
            }
        }
    }
}
