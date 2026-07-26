#define WASM_EXPORT extern "C" __attribute__((visibility("default"))) __attribute__((used))

extern "C" { 
    void csharp_digitalWrite(int pin, int value);
    int csharp_digitalRead(int pin);
    void csharp_pinMode(int pin, int mode);


    int csharp_analogRead(int pin);
    
    
    void csharp_serialPrintStr(const char* text);
    void csharp_serialPrintInt(int num);
}

#pragma region impl

void digitalWrite(int pin, int value) {
    csharp_digitalWrite(pin, value);
}
int digitalRead(int pin) {
    return csharp_digitalRead(pin);
}
void pinMode(int pin, int mode) {
    csharp_pinMode(pin, mode);
}


int analogRead(int pin) {
    return csharp_analogRead(pin);
}

#pragma endregion

class HardwareSerial {
public:
    void begin(long baudrate) {}
    
    void print(const char* text) {
        csharp_serialPrintStr(text);
    }
    void println(const char* text) {
        csharp_serialPrintStr(text);
        csharp_serialPrintStr("\n");
    }
    void print(int num) {
        csharp_serialPrintInt(num);
    }
    void println(int num) {
        csharp_serialPrintInt(num);
        csharp_serialPrintStr("\n");
    }
};

HardwareSerial Serial;

// {USER_CODE}

WASM_EXPORT void arduino_setup() {
    setup();
}

WASM_EXPORT void arduino_loop() {
    loop();
}