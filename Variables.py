
# Regex cleaning

OpenSession = r'Se abre la sesión a (.+?) (de la mañana|de la tarde|de la noche)\.\n'
EndSession = r'Eran las (.+?) (de la mañana|de la tarde|de la noche)\.\n'

Date = r'\d{1,2} de (enero|febrero|mayo|abril|junio|julio|agosto|setiembre|octubre|noviembre|diciembre) de \d{4}'
Header = r'(.*?)DIARIO DE SESIONES DEL CONGRESO DE LOS DIPUTADOS\nPLENO Y DIPUTACIÓN PERMANENTE\nNúm. \d+ ' \
         + Date + r' Pág. \d+'

NumeroExpediente = r'\(Número\s+de\s+(E|e)xpediente\s+(.*?)\)\.?'

SpeackerTitlePattern = r'[A-ZÀÁÈÉÌÏÍÒÓÙÜÚÑ\·\- ,]+'
SpeackerNamePattern = r' \(.*?\)'
SpeackerTreatmentPattern = r'(La señora |El señor )'
SpeackerPattern = SpeackerTreatmentPattern + '((' + SpeackerTitlePattern + SpeackerNamePattern + r')|(' + SpeackerTitlePattern + ')):'
SpeachPattern = SpeackerPattern + '(.*?)((' + SpeackerPattern + ')|$)'
