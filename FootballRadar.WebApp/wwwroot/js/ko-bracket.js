(function () {
    const raw = JSON.parse(document.getElementById('bracket-data').textContent);
    const S = {};
    for (const [k, v] of Object.entries(raw.rounds)) {
        S[k] = v.map(m => ({
            ...m,
            winner: m.winnerId ?? null,
            homeGoals: m.predictedHomeGoals ?? null,
            awayGoals: m.predictedAwayGoals ?? null,
        }));
    }

    function openModal(rk, idx) {
        const m = S[rk][idx];
        if (!m.homeId || !m.awayId || m.home === '?' || m.away === '?') return;

        // Deadline check
        const kickoff = new Date(m.kickoffUtc);
        const isPast = kickoff <= new Date();

        document.getElementById('modal-home-name').textContent = m.home;
        document.getElementById('modal-away-name').textContent = m.away;
        document.getElementById('modal-home-logo').src = m.homeLogo || '';
        document.getElementById('modal-away-logo').src = m.awayLogo || '';
        document.getElementById('modal-home-logo').style.display = m.homeLogo ? '' : 'none';
        document.getElementById('modal-away-logo').style.display = m.awayLogo ? '' : 'none';
        document.getElementById('modal-home-goals').value = m.homeGoals ?? 0;
        document.getElementById('modal-away-goals').value = m.awayGoals ?? 0;
        document.getElementById('modal-error').style.display = 'none';

        // Anstosszeit anzeigen
        const kickoffStr = kickoff.toLocaleString('de-CH', {
            day: '2-digit', month: '2-digit', year: 'numeric',
            hour: '2-digit', minute: '2-digit'
        });
        document.getElementById('modal-kickoff').textContent = `Anstoss: ${kickoffStr}`;
        document.getElementById('modal-kickoff').className = isPast
            ? 'text-center small text-danger mb-2'
            : 'text-center small text-muted mb-2';

        // Inputs und Speichern sperren falls Vergangenheit
        document.getElementById('modal-home-goals').disabled = isPast;
        document.getElementById('modal-away-goals').disabled = isPast;
        document.getElementById('modal-save').disabled = isPast;

        document.getElementById('modal-save').onclick = () => saveModal(rk, idx);
        document.getElementById('ko-modal').dataset.rk = rk;
        document.getElementById('ko-modal').dataset.idx = idx;

        bootstrap.Modal.getOrCreateInstance(document.getElementById('ko-modal')).show();
    }

    function saveModal(rk, idx) {
        const m = S[rk][idx];
        const hg = parseInt(document.getElementById('modal-home-goals').value);
        const ag = parseInt(document.getElementById('modal-away-goals').value);

        if (isNaN(hg) || isNaN(ag) || hg === ag) {
            document.getElementById('modal-error').style.display = '';
            return;
        }

        document.getElementById('modal-error').style.display = 'none';
        m.homeGoals = hg;
        m.awayGoals = ag;
        m.winner = hg > ag ? m.homeId : m.awayId;

        bootstrap.Modal.getOrCreateInstance(document.getElementById('ko-modal')).hide();
        render();
        saveTip(m.fixtureId, m.winner, hg, ag);
    }

    async function saveTip(fixtureId, winnerId, homeGoals, awayGoals) {
        await fetch('/TippMatch/SaveKoTip', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value ?? ''
            },
            body: JSON.stringify({ fixtureId, winnerId, homeGoals, awayGoals })
        });
    }

    function card(rk, idx, type) {
        const m = S[rk][idx];
        if (!m) return '<div class="match-card">–</div>';

        const hUnk = !m.homeId || m.home === '?';
        const aUnk = !m.awayId || m.away === '?';
        const hW = m.winner === m.homeId;
        const aW = m.winner === m.awayId;
        const hasTip = m.homeGoals !== null && m.awayGoals !== null;
        const isPast = m.kickoffUtc && new Date(m.kickoffUtc) <= new Date();
        const cls = type === 'final' ? 'match-card final-card' : type === 'third' ? 'match-card third-card' : 'match-card';
        const clickable = !hUnk && !aUnk && !isPast;

        let scoreBadge = '';
        if (isPast && hasTip) scoreBadge = `<div class="score-badge locked">🔒 ${m.homeGoals} : ${m.awayGoals}</div>`;
        else if (isPast && !hasTip) scoreBadge = `<div class="score-badge locked">–</div>`;
        else if (!isPast && hasTip) scoreBadge = `<div class="score-badge">${m.homeGoals} : ${m.awayGoals}</div>`;

        return `<div class="${cls}${clickable ? ' match-card-clickable' : ''}" ${clickable ? `onclick="bracketOpen('${rk}',${idx})"` : ''}>
        <div class="team-row${hW ? ' winner' : ''}">
            ${m.homeLogo ? `<img src="${m.homeLogo}" style="width:14px;height:14px;object-fit:contain" />` : ''}
            <span class="tname${hUnk ? ' unknown' : ''}">${m.home}</span>
        </div>
        <div class="team-row${aW ? ' winner' : ''}">
            ${m.awayLogo ? `<img src="${m.awayLogo}" style="width:14px;height:14px;object-fit:contain" />` : ''}
            <span class="tname${aUnk ? ' unknown' : ''}">${m.away}</span>
        </div>
        ${scoreBadge}
    </div>`;
    }

    function conns(n, rev) {
        let h = '';
        for (let i = 0; i < n; i++) {
            if (i % 2 === 0) h += `<div class="conn-wrap" style="flex:2"><div class="conn-pair${rev ? ' conn-pair-rev' : ''}"><div class="top"></div><div class="bot"></div></div></div>`;
            else h += `<div class="conn-wrap" style="flex:2"></div>`;
        }
        return h;
    }

    function render() {
        document.getElementById('ko-bracket').innerHTML = `
    <div style="display:flex;align-items:stretch;flex:1">
        ${col('Rd. 32', 'r32_L')}
        <div class="conn-col" id="conn-r32-r16-L">${conns(8, false)}</div>
        ${col('Rd. 16', 'r16_L')}
        <div class="conn-col" id="conn-r16-qf-L">${conns(4, false)}</div>
        ${col('Viertelfinale', 'qf_L')}
        <div class="conn-col" id="conn-qf-sf-L">${conns(2, false)}</div>
        ${col('Halbfinale', 'sf_L')}
        <div class="conn-col" style="width:14px"><div class="conn-wrap" style="flex:1"><div style="width:14px;height:1px;background:var(--bs-border-color-translucent)"></div></div></div>
    </div>
    <div class="final-col">
        <div class="round-lbl">Finale</div>
        <div style="font-size:18px">🏆</div>
        ${card('final', 0, 'final')}
        <div class="round-lbl" style="margin-top:8px">Platz 3</div>
        ${card('third', 0, 'third')}
    </div>
    <div style="display:flex;align-items:stretch;flex:1;flex-direction:row-reverse">
        ${col('Rd. 32', 'r32_R', 'right')}
        <div class="conn-col" id="conn-r32-r16-R">${conns(8, true)}</div>
        ${col('Rd. 16', 'r16_R', 'right')}
        <div class="conn-col" id="conn-r16-qf-R">${conns(4, true)}</div>
        ${col('Viertelfinale', 'qf_R', 'right')}
        <div class="conn-col" id="conn-qf-sf-R">${conns(2, true)}</div>
        ${col('Halbfinale', 'sf_R', 'right')}
        <div class="conn-col" style="width:14px"><div class="conn-wrap" style="flex:1"><div style="width:14px;height:1px;background:var(--bs-border-color-translucent)"></div></div></div>
    </div>`;

        requestAnimationFrame(drawConnectors);
    }

    function drawConnectors() {
        const bracket = document.getElementById('ko-bracket');
        const bracketTop = bracket.getBoundingClientRect().top;

        function getCardMid(selector) {
            const el = bracket.querySelector(selector);
            if (!el) return null;
            const r = el.getBoundingClientRect();
            return r.top + r.height / 2 - bracketTop;
        }

        function drawPairs(connColId, srcRk, dstRk, rev) {
            const col = document.getElementById(connColId);
            if (!col) return;
            const colRect = col.getBoundingClientRect();
            const colH = colRect.height;

            const wraps = col.querySelectorAll('.conn-wrap');
            const pairs = col.querySelectorAll('.conn-pair');
            let pairIdx = 0;

            const cards = bracket.querySelectorAll(`[data-rk="${srcRk}"] .match-card`);

            for (let i = 0; i < cards.length; i += 2) {
                const c1 = cards[i];
                const c2 = cards[i + 1];
                if (!c1 || !c2) continue;

                const m1 = c1.getBoundingClientRect().top + c1.getBoundingClientRect().height / 2 - colRect.top;
                const m2 = c2.getBoundingClientRect().top + c2.getBoundingClientRect().height / 2 - colRect.top;

                const pair = pairs[pairIdx++];
                if (!pair) continue;

                const height = m2 - m1;
                const top = m1;

                pair.parentElement.style.position = 'absolute';
                pair.parentElement.style.top = top + 'px';
                pair.parentElement.style.height = height + 'px';
                pair.parentElement.style.left = '0';
                pair.parentElement.style.right = '0';
            }

            col.style.position = 'relative';
        }

        drawPairs('conn-r32-r16-L', 'r32_L', 'r16_L', false);
        drawPairs('conn-r32-r16-R', 'r32_R', 'r16_R', true);
        drawPairs('conn-r16-qf-L', 'r16_L', 'qf_L', false);
        drawPairs('conn-r16-qf-R', 'r16_R', 'qf_R', true);
        drawPairs('conn-qf-sf-L', 'qf_L', 'sf_L', false);
        drawPairs('conn-qf-sf-R', 'qf_R', 'sf_R', true);
    }

    function col(label, rk, align) {
        return `<div class="round-col" style="text-align:${align || 'left'}">
            <div class="round-lbl">${label}</div>
            <div class="matches-stack">${S[rk].map((_, i) => `<div class="match-wrap" style="flex:1">${card(rk, i, false)}</div>`).join('')}</div>
        </div>`;
    }



    window.bracketOpen = openModal;
    render();
})();