package com.ceteri.rhythmic;

import com.badlogic.gdx.ApplicationAdapter;
import com.badlogic.gdx.Game;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.*;
import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.g2d.BitmapFont;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.graphics.g2d.freetype.FreeTypeFontGenerator;
import com.badlogic.gdx.graphics.g2d.freetype.FreetypeFontLoader;

import java.awt.*;

public class Rhythmic extends ApplicationAdapter {
	SpriteBatch batch;
	Texture img;
	BitmapFont corefont;

	int dotLength = 0;
	int Boottime = 0;




	
	@Override
	public void create () {
		batch = new SpriteBatch();
		img = new Texture("badlogic.jpg");
		FreeTypeFontGenerator mainFont = new FreeTypeFontGenerator(Gdx.files.internal("fonts/wkmrgoth.ttf"));
		FreeTypeFontGenerator.FreeTypeFontParameter parameter = new FreeTypeFontGenerator.FreeTypeFontParameter();
		parameter.size = 24;
		parameter.color = Color.WHITE;
		corefont = mainFont.generateFont(parameter);
		mainFont.dispose();
		(new Thread(new GameInit())).start();
	}

	@Override
	public void render () {
		Gdx.gl.glClearColor(0, 0, 0, 1);
		Gdx.gl.glClear(GL20.GL_COLOR_BUFFER_BIT);
		batch.begin();
		corefont.draw(batch, "beatmania IIDX CUSTOM MIX", 32, Gdx.graphics.getHeight() - 32);
		String dots = "";
		for (int i = 0; i < dotLength; i++) {
			if(i % 10 == 1) {
				dots += ".";
			}
		}
		corefont.draw(batch, "I/O: " + dots, 32, Gdx.graphics.getHeight() - 75);
		corefont.draw(batch, "NETWORK: SKIP", 32, Gdx.graphics.getHeight() - 99);
		corefont.draw(batch, "BOOT TIME: " + Boottime, 32, 64);
		corefont.draw(batch, "COMPILED AT 2017-02-17 17:37", 32, 88);
		corefont.draw(batch, "Hey look, this isn't a unity game. HAPPY NOW!?", 32, 112);
		dotLength++;
		Boottime++;
		if(dotLength > 30) {
			dotLength = 0;
		}
		batch.end();
	}
	
	@Override
	public void dispose () {
		batch.dispose();
		img.dispose();
	}
}
