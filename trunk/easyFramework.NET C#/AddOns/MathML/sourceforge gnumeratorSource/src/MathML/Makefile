all: MathML.dll

clean: rm -f MathML.dll

CC=mcs

SOURCES = \
	AssemblyInfo.cs \
	MathMLActionElement.cs \
	MathMLAlignGroupElement.cs \
	MathMLAlignMarkElement.cs \
	MathMLAnnotationElement.cs \
	MathMLApplyElement.cs \
	MathMLArgumentsEnumerator.cs \
	MathMLArgumentsList.cs \
	MathMLBvarElement.cs \
	MathMLCaseElement.cs \
	MathMLCiElement.cs \
	MathMLCnElement.cs \
	MathMLConditionElement.cs \
	MathMLContainer.cs \
	MathMLContainerImpl.cs \
	MathMLContentContainer.cs \
	MathMLContentElement.cs \
	MathMLContentToken.cs \
	MathMLCsymbolElement.cs \
	MathMLDeclarationsList.cs \
	MathMLDeclareElement.cs \
	MathMLDocument.cs \
	MathMLElement.cs \
	MathMLEncloseElement.cs \
	MathMLFencedElement.cs \
	MathMLFnElement.cs \
	MathMLFractionElement.cs \
	MathMLGlyphElement.cs \
	MathMLIntervalElement.cs \
	MathMLLabeledRowElement.cs \
	MathMLLambdaElement.cs \
	MathMLListElement.cs \
	MathMLMathElement.cs \
	MathMLMatrixElement.cs \
	MathMLMatrixrowElement.cs \
	MathMLMultiScriptsElement.cs \
	MathMLNodeList.cs \
	MathMLOperatorElement.cs \
	MathMLPaddedElement.cs \
	MathMLPiecewiseElement.cs \
	MathMLPredefinedSymbol.cs \
	MathMLPresentationContainer.cs \
	MathMLPresentationElement.cs \
	MathMLPresentationToken.cs \
	MathMLRadicalElement.cs \
	MathMLScriptElement.cs \
	MathMLSemanticsElement.cs \
	MathMLSeparator.cs \
	MathMLSetElement.cs \
	MathMLSpaceElement.cs \
	MathMLStringLitElement.cs \
	MathMLStyleElement.cs \
	MathMLTableCellElement.cs \
	MathMLTableElement.cs \
	MathMLTableRowElement.cs \
	MathMLUnderOverElement.cs \
	MathMLVectorElement.cs \
	MathMLVisitor.cs \
	MathMLXMLAnnotationElement.cs \
	#

MathML.dll: $(SOURCES)
	$(CC) /target:library /out:../../bin/MathML.dll $(SOURCES)




