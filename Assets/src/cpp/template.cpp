#define WASM_EXPORT __attribute__((visibility(""default""))) extern ""C""

extern "C" { 
    void csharp_digitalWrite(int pin, int value);
    int csharp_analogRead(int pin);
}

void digitalWrite(int pin, int value) {
    csharp_digitalWrite(pin, value);
}

int analogRead(int pin) {
    return csharp_analogRead(pin);
}

// {USER_CODE}

WASM_EXPORT void arduino_setup() {
    setup();
}

WASM_EXPORT void arduino_loop() {
    loop();
}