using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Question {

    public string category;
    public string type;
    public string difficulty;
    public string question;
    public string correct_answer;
    public List<string> incorrect_answers;

}

[Serializable]
public class QuestionList {
    public List<Question> results;
}
