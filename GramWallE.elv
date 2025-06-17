/* Reglas Lexicográficas Auxiliares */


%  /* Reglas Lexicográficas */

VAR ; 
ETIQ ;
NUMBER;
STRING;
PCTRUE;
PCFALSE;
PCGOTO;
OPFLECHA;
OPMAS;
OPMENOS;
OPPOR;
OPDIV;
OPPOT;
OPMODULO;
OPAND;
OPOR;
OPIGUAL;
OPMAYORIGUAL;
OPMENORIGUAL;
OPMAYOR;
OPMENOR;
PARAB;
PARCER;
CORCHAB;
CORCHCER;
COMA;
COMAB;
COMCER;
NEWLINE;
PCSPAWN; 
PCCOLOR;
PCSIZE;
PCDRAWLINE;
PCDRAWCIRCLE;
PCDRAWRECTANGLE;
PCFILL;
PCGETACTUALX;
PCGETACTUALY;
PCGETCANVASSIZE;
PCGETCOLORCOUNT;
PCISBRUSHCOLOR;
PCISBRUSHSIZE;
PCISCANVASCOLOR;



%%  /* Reglas Sintácticas */

begin S ->	<lenguaje> ;

<lenguaje> -> <comandoSpawn> <listaInstr> ;

<comandoSpawn> -> PCSPAWN <PosSpawn> ;

<PosSpawn> -> PARAB <expr> COMA <expr> PARCER;

<listaInstr> -> <listaInstrD> ;

<listaInstrD> -> NEWLINE <instr> <listaInstrD> |
	;

<instr> -> <comando> |
	<asign> |
	<Etiq> |
	<saltoCond>  |
	;

<comando> -> PCCOLOR PARAB STRING PARCER |
	PCSIZE PARAB <datoEnt> PARCER |
	PCDRAWLINE PARAB <datoEnt> COMA <datoEnt> COMA <datoEnt> PARCER |
   	PCDRAWCIRCLE PARAB <datoEnt> COMA <datoEnt> COMA <datoEnt> PARCER |
	PCDRAWRECTANGLE PARAB <datoEnt> COMA <datoEnt> COMA <datoEnt> COMA <datoEnt> COMA <datoEnt> PARCER |
   	PCFILL PARAB PARCER;

<asign> -> VAR OPFLECHA <expr> ;

<expr> ->	<term> <exprD> ;

<exprD> -> <opMasMenosAnd> <term> <exprD> | 
	;

<term> -> <factor> <termD> ;

<termD> -> <opPorDivOr> <factor> <termD> | 
	;
		
<factor> -> <dato> <factorD> ;

<factorD> -> OPPOT <dato> <factorD> | 
	;
            
<dato> -> VAR |
	NUMBER |
	<funcion> |
	<datoBool> |
	PARAB <expr> PARCER	;
    
<datoBool> -> PCTRUE | 
	PCFALSE  ;

<datoEnt> -> VAR | 
	NUMBER ;
            
<opMasMenosAnd> -> OPMAS | 
	OPMENOS | 
	OPAND ;

<opPorDivOr> -> OPPOR | 
	OPDIV | 
	OPOR ;

<opRel> -> OPIGUAL | 
	OPMAYORIGUAL | 
	OPMENORIGUAL | 
	OPMAYOR | 
	OPMENOR ;
    
<funcion> -> PCGETACTUALX PARAB PARCER |
	PCGETACTUALY PARAB PARCER |
	PCGETCANVASSIZE PARAB PARCER |
	PCGETCOLORCOUNT PARAB <Etiq> COMA <datoEnt> COMA <datoEnt> COMA <datoEnt> COMA <datoEnt> PARCER |
	PCISBRUSHCOLOR PARAB <Etiq> PARCER |
	PCISBRUSHSIZE PARAB <datoEnt> PARCER |
	PCISCANVASCOLOR PARAB <Etiq> COMA <datoEnt> COMA <datoEnt> PARCER ;

<saltoCond> -> PCGOTO CORCHAB <Etiq> CORCHCER <CondicionGoto> ;

<CondicionGoto> -> PARAB <expr> <opRel> <expr> PARCER ;

<Etiq> -> ETIQ;

<String> -> STRING;








%%  /* Reglas Semánticas */



%%  /* Operadores de Código Intermedio */



%%

Compilador para un programa Wall-El. Especificaciones: 

Ejemplo:
