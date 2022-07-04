package scxmlgen.Modalities;

import scxmlgen.interfaces.IModality;

/**
 *
 * @author nunof
 */
public enum SecondMod implements IModality {

    CHANGE_SL("[0][ChangeSL]", 1500),
    CHANGE_SR("[1][ChangeSR]", 1500),

    CHANGE_VD("[2][ChangeVD]", 1500),
    CHANGE_VU("[3][ChangeVU]", 1500),

    CHANGE_WL("[4][ChangeWL]", 1500),
    CHANGE_WR("[5][ChangeWR]", 1500),

    MOVE_AL("[6][MoveAAL]", 1500),
    MOVE_AR("[7][MoveAAR]", 1500),
    MOVE_AD("[8][MoveAD]", 1500),
    MOVE_AU("[9][MoveAU]", 1500);

    private String event;
    private int timeout;

    SecondMod(String m, int time) {
        event = m;
        timeout = time;
    }

    @Override
    public int getTimeOut() {
        return timeout;
    }

    @Override
    public String getEventName() {
        // return getModalityName()+"."+event;
        return event;
    }

    @Override
    public String getEvName() {
        return getModalityName().toLowerCase() + event.toLowerCase();
    }

}
