using System;
using System.Collections.Generic;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Threading;

namespace PackCPC {
	public class SaveAsm {
		static public void GenereDatas(StreamWriter sw, byte[] tabByte, int length, int nbOctetsLigne, bool isWord) {
			string line = isWord ? "\tDW\t" : "\tDB\t";
			int nbOctets = 0;

			for (int i = 0; i < length; i++) {
				if (isWord) {
					int adr = tabByte[i] + (tabByte[i + 1] << 8);
					line += "#" + adr.ToString("X4") + ",";
					nbOctets++;
					i++;
			}
				else
				line += "#" + tabByte[i].ToString("X2") + ",";
				if (++nbOctets >= Math.Min(nbOctetsLigne, 64)) {
					sw.WriteLine(line.Substring(0, line.Length - 1));
					line = isWord ? "\tDW\t" : "\tDB\t";
					nbOctets = 0;
				}
			}
			if (nbOctets > 0)
				sw.WriteLine(line.Substring(0, line.Length - 1));

			sw.WriteLine("; Taille totale " + length.ToString() + " octets");
		}

		static public void GenereDepack(StreamWriter sw, Main.PackMethode pkMethode, string jumpLabel = null) {
			switch (pkMethode) {
				case Main.PackMethode.Standard:
					GenereDStd(sw, jumpLabel);
					break;

				case Main.PackMethode.ZX0:
					GenereDZX0(sw, jumpLabel);
					break;

				case Main.PackMethode.ZX0_V2:
					GenereDZX0_V2(sw, jumpLabel);
					break;

				case Main.PackMethode.ZX1:
					GenereDZX1(sw, jumpLabel);
					break;
			}
		}

		static private void GenereDZX0(StreamWriter sw, string jumpLabel = null) {
			sw.WriteLine("; Decompactage");
			sw.WriteLine("Depack");
			sw.WriteLine("	ld	bc,#ffff			; preserve default offset 1");
			sw.WriteLine("	push	bc");
			sw.WriteLine("	inc	bc");
			sw.WriteLine("	ld	a,#80");
			sw.WriteLine("dzx0s_literals");
			sw.WriteLine("	call	dzx0s_elias		; obtain length");
			sw.WriteLine("	ldir					; copy literals");
			sw.WriteLine("	add	a,a					; copy from last offset or new offset?");
			sw.WriteLine("	jr	c,dzx0s_new_offset");
			sw.WriteLine("	call	dzx0s_elias		; obtain length");
			sw.WriteLine("dzx0s_copy");
			sw.WriteLine("	ex	(sp),hl				; preserve source,restore offset");
			sw.WriteLine("	push	hl				; preserve offset");
			sw.WriteLine("	add	hl,de				; calculate destination - offset");
			sw.WriteLine("	ldir					; copy from offset");
			sw.WriteLine("	pop	hl					; restore offset");
			sw.WriteLine("	ex	(sp),hl				; preserve offset,restore source");
			sw.WriteLine("	add	a,a					; copy from literals or new offset?");
			sw.WriteLine("	jr	nc,dzx0s_literals");
			sw.WriteLine("dzx0s_new_offset");
			sw.WriteLine("	call	dzx0s_elias		; obtain offset MSB");
			sw.WriteLine("	ld b,a");
			sw.WriteLine("	pop	af					; discard last offset");
			sw.WriteLine("	xor	a					; adjust for negative offset");
			sw.WriteLine("	sub	c");
			sw.WriteLine((jumpLabel != null ? ("	JP	Z," + jumpLabel) : "	RET	Z") + "		; Plus d'octets à traiter = fini" + Environment.NewLine);
			sw.WriteLine("	ld	c,a");
			sw.WriteLine("	ld	a,b");
			sw.WriteLine("	ld	b,c");
			sw.WriteLine("	ld	c,(hl)				; obtain offset LSB");
			sw.WriteLine("	inc	hl");
			sw.WriteLine("	rr	b					; last offset bit becomes first length bit");
			sw.WriteLine("	rr	c");
			sw.WriteLine("	push	bc				; preserve new offset");
			sw.WriteLine("	ld	bc,1				; obtain length");
			sw.WriteLine("	call	nc,dzx0s_elias_backtrack");
			sw.WriteLine("	inc	bc");
			sw.WriteLine("	jr	dzx0s_copy");
			sw.WriteLine("dzx0s_elias");
			sw.WriteLine("	inc	c					; interlaced Elias gamma coding");
			sw.WriteLine("dzx0s_elias_loop");
			sw.WriteLine("	add	a,a");
			sw.WriteLine("	jr	nz,dzx0s_elias_skip");
			sw.WriteLine("	ld	a,(hl)				; load another group of 8 bits");
			sw.WriteLine("	inc	hl");
			sw.WriteLine("	rla");
			sw.WriteLine("dzx0s_elias_skip");
			sw.WriteLine("	ret 	c");
			sw.WriteLine("dzx0s_elias_backtrack");
			sw.WriteLine("	add	a,a");
			sw.WriteLine("	rl	c");
			sw.WriteLine("	rl	b");
			sw.WriteLine("	jr	dzx0s_elias_loop");
		}

		static private void GenereDZX0_V2(StreamWriter sw, string jumpLabel=null) {
			sw.WriteLine("; Decompactage");
			sw.WriteLine("Depack");
			sw.WriteLine("ld bc,#ffff				; preserve default offset 1");
			sw.WriteLine("	push	bc");
			sw.WriteLine("	inc	bc");
			sw.WriteLine("	ld	a,#80");
			sw.WriteLine("dzx0s_literals");
			sw.WriteLine("	call	dzx0s_elias		;obtain length");
			sw.WriteLine("	ldir					;copy literals");
			sw.WriteLine("	add	a,a					;copy from last offset or new offset?");
			sw.WriteLine("	jr	c,dzx0s_new_offset");
			sw.WriteLine("	call	dzx0s_elias		;obtain length");
			sw.WriteLine("dzx0s_copy");
			sw.WriteLine("	ex	(sp),hl				;preserve source, restore offset");
			sw.WriteLine("	push	hl				;preserve offset");
			sw.WriteLine("	add	hl,de				;calculate destination -offset");
			sw.WriteLine("	ldir					;copy from offset");
			sw.WriteLine("	pop	hl					;restore offset");
			sw.WriteLine("	ex	(sp),hl				;preserve offset, restore source");
			sw.WriteLine("	add	a,a					;copy from literals or new offset?");
			sw.WriteLine("	jr	nc,dzx0s_literals");
			sw.WriteLine("dzx0s_new_offset");
			sw.WriteLine("	pop	bc					;discard last offset");
			sw.WriteLine("	ld	c,#fe				; prepare negative offset");
			sw.WriteLine("	call dzx0s_elias_loop	;obtain offset MSB");
			sw.WriteLine("	inc	c");
			sw.WriteLine((jumpLabel != null ? ("	JP	Z," + jumpLabel) : "	RET	Z") + "		; Plus d'octets à traiter = fini" + Environment.NewLine);
			sw.WriteLine("	ld	b,c");
			sw.WriteLine("	ld	c,(hl)				;obtain offset LSB");
			sw.WriteLine("	inc	hl");
			sw.WriteLine("	rr	b					;last offset bit becomes first length bit");
			sw.WriteLine("	rr	c");
			sw.WriteLine("	push	bc				;preserve new offset");
			sw.WriteLine("	ld	bc,1				;obtain length");
			sw.WriteLine("	call	nc,dzx0s_elias_backtrack");
			sw.WriteLine("	inc	bc");
			sw.WriteLine("	jr	dzx0s_copy");
			sw.WriteLine("dzx0s_elias");
			sw.WriteLine("	inc	c					;interlaced Elias gamma coding");
			sw.WriteLine("dzx0s_elias_loop");
			sw.WriteLine("	add	a,a");
			sw.WriteLine("	jr	nz,dzx0s_elias_skip");
			sw.WriteLine("	ld	a,(hl)				 ;load another group of 8 bits");
			sw.WriteLine("	inc	hl");
			sw.WriteLine("	rla");
			sw.WriteLine("dzx0s_elias_skip");
			sw.WriteLine("	ret	c");
			sw.WriteLine("dzx0s_elias_backtrack");
			sw.WriteLine("	add	a,a");
			sw.WriteLine("	rl	c");
			sw.WriteLine("	rl	b");
			sw.WriteLine("	jr	dzx0s_elias_loop");
		}

		static private void GenereDZX1(StreamWriter sw, string jumpLabel = null) {
			sw.WriteLine("; Decompactage");
			sw.WriteLine("Depack");
			sw.WriteLine("	ld	bc,#ffff			; preserve default offset 1");
			sw.WriteLine("	push	bc");
			sw.WriteLine("	ld	a,#80");
			sw.WriteLine("dzx1s_literals");
			sw.WriteLine("	call	dzx1s_elias		; obtain length");
			sw.WriteLine("	ldir					; copy literals");
			sw.WriteLine("	add	a,a					; copy from last offset or new offset?");
			sw.WriteLine("	jr	c,dzx1s_new_offset");
			sw.WriteLine("	call	dzx1s_elias		; obtain length");
			sw.WriteLine("dzx1s_copy");
			sw.WriteLine("	ex	(sp),hl				; preserve source,restore offset");
			sw.WriteLine("	push	hl				; preserve offset");
			sw.WriteLine("	add	hl,de				; calculate destination - offset");
			sw.WriteLine("	ldir					; copy from offset");
			sw.WriteLine("	pop	hl					; restore offset");
			sw.WriteLine("	ex	(sp),hl				; preserve offset,restore source");
			sw.WriteLine("	add	a,a					; copy from literals or new offset?");
			sw.WriteLine("	jr	nc,dzx1s_literals");
			sw.WriteLine("dzx1s_new_offset");
			sw.WriteLine("	inc	sp					; discard last offset");
			sw.WriteLine("	inc	sp");
			sw.WriteLine("	dec	b");
			sw.WriteLine("	ld	c,(hl)				; obtain offset LSB");
			sw.WriteLine("	inc	hl");
			sw.WriteLine("	rr	c					; single byte offset?");
			sw.WriteLine("	jr	nc,dzx1s_msb_skip");
			sw.WriteLine("	ld	b,(hl)				; obtain offset MSB");
			sw.WriteLine("	inc	hl");
			sw.WriteLine("	rr	b					; replace last LSB bit with last MSB bit");
			sw.WriteLine("	inc	b");
			sw.WriteLine((jumpLabel != null ? ("	JP	Z," + jumpLabel) : "	RET	Z") + "		; Plus d'octets à traiter = fini" + Environment.NewLine);
			sw.WriteLine("	rl	c");
			sw.WriteLine("dzx1s_msb_skip");
			sw.WriteLine("	push	bc				; preserve new offset");
			sw.WriteLine("	call	dzx1s_elias		; obtain length");
			sw.WriteLine("	inc	bc");
			sw.WriteLine("	jr	dzx1s_copy");
			sw.WriteLine("dzx1s_elias");
			sw.WriteLine("	ld	bc,1				; interlaced Elias gamma coding");
			sw.WriteLine("dzx1s_elias_loop");
			sw.WriteLine("	add	a,a");
			sw.WriteLine("	jr	nz,dzx1s_elias_skip");
			sw.WriteLine("	ld	a,(hl)				; load another group of 8 bits");
			sw.WriteLine("	inc	hl");
			sw.WriteLine("	rla");
			sw.WriteLine("dzx1s_elias_skip");
			sw.WriteLine("	ret	nc");
			sw.WriteLine("	add	a,a");
			sw.WriteLine("	rl	c");
			sw.WriteLine("	rl	b");
			sw.WriteLine("	jr	dzx1s_elias_loop");
		}

		static private void GenereDStd(StreamWriter sw, string jumpLabel = null) {
			sw.WriteLine("; Decompactage");
			sw.WriteLine("Depack");
			sw.WriteLine("	LD	A,(HL)			; DepackBits = InBuf[ InBytes++ ]");
			sw.WriteLine("	INC	HL");
			sw.WriteLine("	RRA				; Rotation rapide calcul seulement flag C");
			sw.WriteLine("	SET	7,A			; Positionne bit 7 en gardant flag C");
			sw.WriteLine("	LD	(BclLzw+1),A");
			sw.WriteLine("	JR	C,TstCodeLzw");
			sw.WriteLine("CopByteLzw");
			sw.WriteLine("	LDI				; OutBuf[ OutBytes++ ] = InBuf[ InBytes++ ]" + Environment.NewLine);
			sw.WriteLine("BclLzw");
			sw.WriteLine("	LD	A,0");
			sw.WriteLine("	RR	A			; Rotation avec calcul Flags C et Z");
			sw.WriteLine("	LD	(BclLzw+1),A");
			sw.WriteLine("	JR	NC,CopByteLzw");
			sw.WriteLine("	JR	Z,Depack" + Environment.NewLine);
			sw.WriteLine("TstCodeLzw");
			sw.WriteLine("	LD	A,(HL)			; A = InBuf[ InBytes ];");
			sw.WriteLine("	AND	A");
			sw.WriteLine((jumpLabel != null ? ("	JP	Z," + jumpLabel) : "	RET	Z") + "		; Plus d'octets à traiter = fini" + Environment.NewLine);
			sw.WriteLine("	INC	HL");
			sw.WriteLine("	LD	B,A			; B = InBuf[ InBytes ]");
			sw.WriteLine("	RLCA				; A & #80 ?");
			sw.WriteLine("	JR	NC,TstLzw40" + Environment.NewLine);
			sw.WriteLine("	RLCA");
			sw.WriteLine("	RLCA");
			sw.WriteLine("	RLCA");
			sw.WriteLine("	AND	7");
			sw.WriteLine("	ADD	A,3			; Longueur = 3 + ( ( InBuf[ InBytes ] >> 4 ) & 7 );");
			sw.WriteLine("	LD	C,A			; C = Longueur");
			sw.WriteLine("	LD	A,B			; B = InBuf[InBytes]");
			sw.WriteLine("	AND	#0F			; Delta = ( InBuf[ InBytes++ ] & 15 ) << 8");
			sw.WriteLine("	LD	B,A			; B = poids fort Delta");
			sw.WriteLine("	LD	A,C			; A = Length");
			sw.WriteLine("	SCF				; Repositionner flag C (pour Delta++)");
			sw.WriteLine("CopyBytes0");
			sw.WriteLine("	LD	C,(HL)			; C = poids faible Delta (Delta |= InBuf[ InBytes++ ]);");
			sw.WriteLine("	INC	HL");
			sw.WriteLine("	PUSH	HL");
			sw.WriteLine("	LD	H,D");
			sw.WriteLine("	LD	L,E");
			sw.WriteLine("	SBC	HL,BC			; HL=HL-(BC+1)");
			sw.WriteLine("	LD	B,0");
			sw.WriteLine("CopyBytes1");
			sw.WriteLine("	LD	C,A");
			sw.WriteLine("CopyBytes2");
			sw.WriteLine("	LDIR");
			sw.WriteLine("CopyBytes3");
			sw.WriteLine("	POP	HL");
			sw.WriteLine("	JR	BclLzw" + Environment.NewLine);
			sw.WriteLine("TstLzw40");
			sw.WriteLine("	RLCA				; A & #40 ?");
			sw.WriteLine("	JR	NC,TstLzw20" + Environment.NewLine);
			sw.WriteLine("	LD	C,B");
			sw.WriteLine("	RES	6,C			; Delta = 1 + InBuf[ InBytes++ ] & #3f;");
			sw.WriteLine("	LD	B,0			; BC = Delta + 1 car flag C = 1");
			sw.WriteLine("	PUSH	HL");
			sw.WriteLine("	LD	H,D");
			sw.WriteLine("	LD	L,E");
			sw.WriteLine("	SBC	HL,BC");
			sw.WriteLine("	LDI");
			sw.WriteLine("	LDI				; Longueur = 2");
			sw.WriteLine("	JR	CopyBytes3" + Environment.NewLine);
			sw.WriteLine("TstLzw20");
			sw.WriteLine("	RLCA				; A & #20 ?");
			sw.WriteLine("	JR	NC,TstLzw10" + Environment.NewLine);
			sw.WriteLine("	LD	A,B			; B compris entre #20 et #3F");
			sw.WriteLine("	ADD	A,#E2			; = ( A AND #1F ) + 2,et positionne carry");
			sw.WriteLine("	LD	B,0			; Longueur = 2 + ( InBuf[ InBytes++ ] & 31 );");
			sw.WriteLine("	JR	CopyBytes0" + Environment.NewLine);
			sw.WriteLine("CodeLzw0F");
			sw.WriteLine("	LD	C,(HL)");
			sw.WriteLine("	PUSH	HL");
			sw.WriteLine("	LD	H,D");
			sw.WriteLine("	LD	L,E");
			sw.WriteLine("	CP	#F0");
			sw.WriteLine("	JR	NZ,CodeLzw02" + Environment.NewLine);
			sw.WriteLine("	XOR	A");
			sw.WriteLine("	LD	B,A");
			sw.WriteLine("	INC	BC			; Longueur = Delta = InBuf[ InBytes + 1 ] + 1;");
			sw.WriteLine("	SBC	HL,BC");
			sw.WriteLine("	LDIR");
			sw.WriteLine("	POP	HL");
			sw.WriteLine("	INC	HL			; Inbytes += 2");
			sw.WriteLine("	JR	BclLzw" + Environment.NewLine);
			sw.WriteLine("CodeLzw02");
			sw.WriteLine("	CP	#20");
			sw.WriteLine("	JR	C,CodeLzw01" + Environment.NewLine);
			sw.WriteLine("	LD	C,B			; Longueur = Delta = InBuf[ InBytes ];");
			sw.WriteLine("	LD	B,0");
			sw.WriteLine("	SBC	HL,BC");
			sw.WriteLine("	JR	CopyBytes2" + Environment.NewLine);
			sw.WriteLine("CodeLzw01				; Ici,B = 1");
			sw.WriteLine("	XOR	A			; Carry a zéro");
			sw.WriteLine("	DEC	H			; Longueur = Delta = 256");
			sw.WriteLine("	JR	CopyBytes1" + Environment.NewLine);
			sw.WriteLine("TstLzw10");
			sw.WriteLine("	RLCA				; A & #10 ?");
			sw.WriteLine("	JR	NC,CodeLzw0F" + Environment.NewLine);
			sw.WriteLine("	RES	4,B			; B = Delta(high) -> ( InBuf[ InBytes++ ] & 15 ) << 8;");
			sw.WriteLine("	LD	C,(HL)			; C = Delta(low)  -> InBuf[ InBytes++ ];");
			sw.WriteLine("	INC	HL");
			sw.WriteLine("	LD	A,(HL)			; A = Longueur - 1");
			sw.WriteLine("	INC	HL");
			sw.WriteLine("	PUSH	HL");
			sw.WriteLine("	LD	H,D");
			sw.WriteLine("	LD	L,E");
			sw.WriteLine("	SBC	HL,BC			; Flag C=1 -> hl=hl-(bc+1) (Delta+1)");
			sw.WriteLine("	LD	B,0");
			sw.WriteLine("	LD	C,A");
			sw.WriteLine("	INC	BC			; BC =  Longueur = InBuf[ InBytes++ ] + 1;");
			sw.WriteLine("	JR	CopyBytes2" + Environment.NewLine);
		}
	}
}
