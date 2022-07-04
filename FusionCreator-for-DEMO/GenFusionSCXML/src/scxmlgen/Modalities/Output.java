package scxmlgen.Modalities;

import scxmlgen.interfaces.IOutput;

public enum Output implements IOutput {

    CHANGE_SL("[target][SLIDES][action][LEFT][value][]"),
    CHANGE_SR("[target][SLIDES][action][RIGHT][value][]"),

    CHANGE_VF("[target][VOLUME][action][.][value][100]"),
    CHANGE_VD("[target][VOLUME][action][-][value][]"),
    CHANGE_VU("[target][VOLUME][action][+][value][]"),

    WORKSPACE_PREV("[target][WORKSPACE][action][MOVE][value][PREV]"),
    WORKSPACE_NEXT("[target][WORKSPACE][action][MOVE][value][NEXT]"),

    NOTEPAD_LEFT("[target][NOTEPAD][action][MOVE][value][LEFT]"),
    NOTEPAD_RIGHT("[target][NOTEPAD][action][MOVE][value][RIGHT]"),
    NOTEPAD_DOWN("[target][NOTEPAD][action][MOVE][value][DOWN]"),
    NOTEPAD_UP("[target][NOTEPAD][action][MOVE][value][UP]"),

    CALC_LEFT("[target][CALC][action][MOVE][value][LEFT]"),
    CALC_RIGHT("[target][CALC][action][MOVE][value][RIGHT]"),
    CALC_DOWN("[target][CALC][action][MOVE][value][DOWN]"),
    CALC_UP("[target][CALC][action][MOVE][value][UP]"),


    BRIGHT("[target][BRIGHT][action][100][value][100]");


    private String event;

    Output(String m) {
        event = m;
    }

    public String getEvent() {
        return this.toString();
    }

    public String getEventName() {
        return event;
    }
}
