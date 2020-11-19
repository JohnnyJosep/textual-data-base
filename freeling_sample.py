import subprocess

def freeling(text, port):
    command = f'echo {text} | analyzer_client.exe {port}'

    process = subprocess.Popen(command, stdout=subprocess.PIPE, stderr=None, shell=True)
    out, err = process.communicate()
    out = out.decode('utf-8').strip()
    return out

print(freeling("Hola mundo", 50005))