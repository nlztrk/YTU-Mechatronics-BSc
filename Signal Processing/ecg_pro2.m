filename = 'noisy_ecg.mat';
myVars = {'Ts','ecg','fs'};
S = load(filename,myVars{:});

Ts=0.002;
fs=1/Ts;
wm=pi*fs;

wc=2*pi*(40/fs);

n=-30:30;
h=sinc(wc*n/pi)*wc/pi

figure(1)

t=0:Ts:9.998;
subplot(3,1,1)
plot(t,S.ecg);
xlabel('Time') % x-axis label
ylabel('Amplitude') % y-axis label

y=filtfilt(h,1,S.ecg);
subplot(3,1,2);
plot(t,y);
xlabel('Time') % x-axis label
ylabel('Amplitude') % y-axis label

a=filter(h,1,S.ecg);
subplot(3,1,3);
plot(t,a);
xlabel('Time') % x-axis label
ylabel('Amplitude') % y-axis label


figure(2)

imp = [1; zeros(180,1)];
impresponse_filter=filter(h,1,imp);
b=stem(0:180,impresponse_filter);
xlabel('Samples') % x-axis label
ylabel('Amplitude') % y-axis label

figure(3)

freqz(h);

for k = 1:61
    fprintf('a(%d) = %8.6g\n', k-1, h(k));
end
