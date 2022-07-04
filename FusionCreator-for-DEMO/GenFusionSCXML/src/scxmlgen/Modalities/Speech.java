/* 
  *   Speech.java generated by speechmod 
 */

package scxmlgen.Modalities;

import scxmlgen.interfaces.IModality;

public enum Speech implements IModality {

	// bright
    BRIGHT_INC("[target][BRIGHT][action][+][value][]", 1500),
    BRIGHT_DEC("[target][BRIGHT][action][-][value][]", 1500),
    
	// volume
	VOLUME_UP("[target][VOLUME][action][+][value][]", 1500),
    VOLUME_DOWN("[target][VOLUME][action][-][value][]", 1500),
    VOLUME_FULL("[target][VOLUME][action][.][value][100]", 1500),
    
	// workspace
	WORKSPACE_PREV("[target][WORKSPACE][action][MOVE][value][PREV]", 1500),
    WORKSPACE_NEXT("[target][WORKSPACE][action][MOVE][value][NEXT]", 1500),
    
	// move notepad
	MOVE_ALEFT_NOTEPAD("[target][NOTEPAD][action][MOVE][value][LEFT]", 5000),
    MOVE_ARIGHT_NOTEPAD("[target][NOTEPAD][action][MOVE][value][RIGHT]", 5000),
    MOVE_ADOWN_NOTEPAD("[target][NOTEPAD][action][MOVE][value][DOWN]", 5000),
    MOVE_AUP_NOTEPAD("[target][NOTEPAD][action][MOVE][value][UP]", 5000),

	// move calculator
	MOVE_ALEFT_CALC("[target][CALC][action][MOVE][value][LEFT]", 5000),
    MOVE_ARIGHT_CALC("[target][CALC][action][MOVE][value][RIGHT]", 5000),
    MOVE_ADOWN_CALC("[target][CALC][action][MOVE][value][DOWN]", 5000),
    MOVE_AUP_CALC("[target][CALC][action][MOVE][value][UP]", 5000),
    
	// fusion: choose notepad or calc to move
	NOTEPAD("[target][NOTEPAD][action][][value][]", 5000),
    CALC("[target][CALC][action][][value][]", 5000);

    
	private String event;
	private int timeout;

	Speech(String m, int time) {
		event = m;
		timeout = time;
	}

	@Override
	public int getTimeOut() {
		return timeout;
	}

	@Override
	public String getEventName() {
		return event;
	}

	@Override
	public String getEvName() {
		return getModalityName().toLowerCase() + event.toLowerCase();
	}

}
