package com.ceteri.rhythmic;

/**
 * Created by Namwen on 17/02/2017.
 */
public class GameInit implements Runnable{

    int PT_Count = 0;
    int PT_Main = 0;
    static String PT_Str = "";

    public void run() {
        while (true) {
            PT_Count++;
            if(PT_Count == 5000000) {
                PT_Count = 0;
                PT_Main++;
            }

            PT_Str = PT_Main + " cycles, curval = " + PT_Count + ", left = " + (5000000 - PT_Count);
        }
    }

    public void ProcessTest() {
        while (true) {
            PT_Count++;
            if(PT_Count == 500) {
                PT_Count = 0;
                PT_Main++;
            }

            PT_Str = PT_Main + " cycles, curval = " + PT_Count + ", left = " + (500 - PT_Count);
        }
    }
}
