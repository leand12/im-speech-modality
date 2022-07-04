/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package genfusionscxml;

import java.io.IOException;
import scxmlgen.Fusion.FusionGenerator;
import scxmlgen.Modalities.Output;
import scxmlgen.Modalities.Speech;
import scxmlgen.Modalities.SecondMod;

/**
 *
 * @author nunof
 */
public class GenFusionSCXML {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) throws IOException {

        FusionGenerator fg = new FusionGenerator();

        // Slides
        fg.Single(SecondMod.CHANGE_SL, Output.CHANGE_SL);
        fg.Single(SecondMod.CHANGE_SR, Output.CHANGE_SR);
        
        // Volume
        fg.Single(Speech.VOLUME_FULL, Output.CHANGE_VF);
        fg.Redundancy(Speech.VOLUME_DOWN, SecondMod.CHANGE_VD, Output.CHANGE_VD);
        fg.Redundancy(Speech.VOLUME_UP, SecondMod.CHANGE_VU, Output.CHANGE_VU);

        // Workspace
        fg.Redundancy(Speech.WORKSPACE_PREV, SecondMod.CHANGE_WL, Output.WORKSPACE_PREV);
        fg.Redundancy(Speech.WORKSPACE_NEXT, SecondMod.CHANGE_WR, Output.WORKSPACE_NEXT);

        // Voice: Move Notepad
        fg.Single(Speech.MOVE_AUP_NOTEPAD, Output.NOTEPAD_UP);
        fg.Single(Speech.MOVE_ADOWN_NOTEPAD, Output.NOTEPAD_DOWN);
        fg.Single(Speech.MOVE_ALEFT_NOTEPAD, Output.NOTEPAD_LEFT);
        fg.Single(Speech.MOVE_ARIGHT_NOTEPAD, Output.NOTEPAD_RIGHT);
        
        // Voice: Move Calculator
        fg.Single(Speech.MOVE_AUP_CALC, Output.CALC_UP);
        fg.Single(Speech.MOVE_ADOWN_CALC, Output.CALC_DOWN);
        fg.Single(Speech.MOVE_ALEFT_CALC, Output.CALC_LEFT);
        fg.Single(Speech.MOVE_ARIGHT_CALC, Output.CALC_RIGHT);
        
        // Fusion: Move Notepad
        fg.Complementary(Speech.NOTEPAD, SecondMod.MOVE_AU , Output.NOTEPAD_UP);
        fg.Complementary(Speech.NOTEPAD, SecondMod.MOVE_AD , Output.NOTEPAD_DOWN);
        fg.Complementary(Speech.NOTEPAD, SecondMod.MOVE_AL , Output.NOTEPAD_LEFT);
        fg.Complementary(Speech.NOTEPAD, SecondMod.MOVE_AR , Output.NOTEPAD_RIGHT);

        // Fusion: Move Calculator
        fg.Complementary(Speech.CALC, SecondMod.MOVE_AU , Output.CALC_UP);
        fg.Complementary(Speech.CALC, SecondMod.MOVE_AD , Output.CALC_DOWN);
        fg.Complementary(Speech.CALC, SecondMod.MOVE_AL , Output.CALC_LEFT);
        fg.Complementary(Speech.CALC, SecondMod.MOVE_AR , Output.CALC_RIGHT);


        // fg.Single(Speech.CIRCLE, Output.CIRCLE); //EXAMPLE
        // fg.Redundancy(Speech.CIRCLE, SecondMod.CIRCLE, Output.CIRCLE); //EXAMPLE

        fg.Build("fusion.scxml");

    }

}
