%% Initialize parameters - DON'T EDIT
load question2_signal;
load low_pass_coefficients;
Fs = 44100; Ts = 1/Fs;
%
%% Place your code here... - EDIT HERE
wp = 2*200*Ts; % Pass-band edge frequency
ws = 2*400*Ts; % Stop-band edge frequency
[h,w]=freqz(bL,aL); % The magnitude and phase frequency values from the frequency analysis.

% Plot the input and output in two subplots
figure(1)

subplot(2,1,1)
plot(t, x); % The graph of the value of the input signal.
xlabel('Seconds'); ylabel('Input');

subplot(2,1,2)
y=filtfilt(bL,aL,x); % Filtering the input signal with Butterworth digital filter.
plot(t,y); % The graph of the value of the filtered signal (output).
xlabel('Seconds'); ylabel('Output');

% Plot the input and output Fourier plots (FFT plots) in two subplots

expanded_w=w(1):(w(512)-w(1))/4410:w(512); % Expanding of frequency array to the same length as input signal's matrix has.
maxfrequency=expanded_w/(2*pi*Ts); % Setting the new frequency value of recently expanded frequency matrix's length.

figure(2)

input_FFT = fft(x); % The DTFT of the input signal.

subplot(2,1,1)
plot(maxfrequency, input_FFT); % The graph of DTFT of the input signal respect to the frequency
xlabel('Frequency'); ylabel('Input DTFT');

output_FFT = fft(y); % The DTFT of the output signal.

subplot(2,1,2)
plot(maxfrequency, output_FFT); % The graph of DTFT of the output signal respect to the frequency
xlabel('Frequency'); ylabel('Output DTFT');

% End of your code - END OF EDIT